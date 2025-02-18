using System;
using System.Collections.Generic;

namespace ThomasonAlgorithm.Tools;

public class CubicGraphWithChords
{
    public int N { get; private set; }
    // adjacencyList[v] хранит список соседей вершины v
    public List<int>[] AdjacencyList { get; private set; }

    public int MaxChordLength = -1;
    public (int, int) MaxChord;

    public CubicGraphWithChords(int n)
    {
        N = n;
        AdjacencyList = new List<int>[n];
        for (int i = 0; i < n; i++)
        {
            AdjacencyList[i] = new List<int>();
        }
    }

    // Добавление неориентированного ребра (u, v)
    public void AddEdge(int u, int v)
    {
        AdjacencyList[u].Add(v);
        AdjacencyList[v].Add(u);
    }

    // Вывод для контроля (можно заменить на вывод в файл и т.п.)
    public void PrintEdges()
    {
        var edgesPrinted = new HashSet<(int,int)>();
        for (int i = 0; i < N; i++)
        {
            foreach (int j in AdjacencyList[i])
            {
                // Чтобы не печатать каждое ребро дважды, упорядочим пару
                if (!edgesPrinted.Contains((Math.Min(i,j), Math.Max(i,j))))
                {
                    Console.WriteLine($"{i} -- {j}");
                    edgesPrinted.Add((Math.Min(i,j), Math.Max(i,j)));
                }
            }
        }
    }

    public int[,] GetAdjacencyMatrix()
    {
        var adjacencyMatrix = new int[N, N];

        for (int i = 0; i < N; i++)
        {
            foreach (var neighbor in AdjacencyList[i])
            {
                adjacencyMatrix[i, neighbor] = 1;
            }
        }
        
        return adjacencyMatrix;
    }

    // Проверка: степень каждой вершины == 3?
    public bool IsCubic()
    {
        for (int i = 0; i < N; i++)
        {
            if (AdjacencyList[i].Count != 3)
                return false;
        }
        return true;
    }
}

public static class CubicGraphGenerator
{
    private static Random rand = new Random();

    /// <summary>
    /// Строит случайный 3-регулярный граф на n вершинах,
    /// гарантируя, что каждая хорда цикла имеет длину не более k.
    /// Возвращает null, если не удалось найти паросочетание после заданного числа попыток.
    /// </summary>
    public static CubicGraphWithChords Generate(int n, int k, int maxAttempts = 1000)
    {
        // 1. Проверим базовые условия
        if (n < 4 || n % 2 != 0) 
            throw new ArgumentException("3-регулярный граф возможен только при четном n >= 4.");
        if (k < 1 || k > n/2)
            throw new ArgumentException("Некорректное значение k.");

        for (int attempt = 0; attempt < maxAttempts; attempt++)
        {
            var graph = new CubicGraphWithChords(n);
            
            // 2. Создаём цикл: 0--1, 1--2, ..., (n-2)--(n-1), (n-1)--0
            for (int i = 0; i < n; i++)
            {
                int j = (i + 1) % n; 
                graph.AddEdge(i, j);
            }

            // 3. Сформируем список всех "допустимых" рёбер (хорд), 
            //    которые могут быть добавлены, чтобы не превышать длину k.
            var chordCandidates = new List<(int, int)>();
            var possibleNeighbors = new Dictionary<int, HashSet<int>>();

            for (int i = 0; i < n; i++)
            {
                possibleNeighbors.Add(i, []);
            }
            
            for (int i = 0; i < n; i++)
            {
                for (int j = i+2; j < n; j++)
                {
                    if (j == (i - 1 + n) % n) 
                        continue; // сосед по циклу (конец сегмента)
                    
                    int dist = Math.Min(Math.Abs(j - i), n - Math.Abs(j - i));
                    if (dist <= k)
                    {
                        chordCandidates.Add((i, j));
                        possibleNeighbors[i].Add(j);
                        possibleNeighbors[j].Add(i);
                    }
                }
            }

            // Теперь нужно найти паросочетание в этом множестве chordCandidates,
            // чтобы каждая вершина была выбрана ровно один раз.
            // Попробуем рандомизированный бэктрекинг.

            // Перемешаем кандидатов, чтобы граф был "случайным"
            ////////
            // Shuffle(chordCandidates);
            ///////

            // Найдём совершенное паросочетание
            var matching = FindPerfectMatching(n, chordCandidates, possibleNeighbors, graph);

            if (matching != null)
            {
                // matching – это список пар (u,v). Добавим их в граф
                foreach (var (u, v) in matching)
                {
                    graph.AdjacencyList[u].Add(v);
                    graph.AdjacencyList[v].Add(u);
                }

                // Проверим, получилась ли у нас действительно степень 3 у каждой вершины
                if (graph.IsCubic())
                    return graph;
                // Иначе пробуем заново
            }
        }

        // Если не смогли найти ни разу – вернём null или бросим исключение.
        return null;
    }

    // Рандомизированная функция поиска совершенного паросочетания (упрощённо)
    // chordCandidates: все возможные хорды (u,v), удовлетворя dist(u,v) <= k
    // graph: уже содержит рёбра цикла (чтобы мы не делали дубликатов и не нарушали степень)
    private static List<(int,int)> FindPerfectMatching(
        int n, 
        List<(int,int)> chordCandidates,
        Dictionary<int, HashSet<int>> possibleNeighbors,
        CubicGraphWithChords graph)
    {
        // Сколько рёбер ещё нужно "добавить"? Каждой вершине нужен ровно 1 "лишний" сосед,
        // но у неё его может ещё не быть (или уже быть, если случайно образовались дубликаты).
        // Фактически, нам нужно "спарить" те вершины, у которых AdjacencyList[v].Count < 3.

        // Массив, показывающий, сколько ещё рёбер "не хватает" вершине v (чтобы степень была 3)
        int[] needed = new int[n];
        int totalNeeded = 0;
        for (int i = 0; i < n; i++)
        {
            int deg = graph.AdjacencyList[i].Count;
            if (deg > 3) 
            {
                // Ошибка: где-то уже нарушили кубичность
                return null;
            }
            needed[i] = 3 - deg; 
            totalNeeded += needed[i];
        }

        // totalNeeded должно быть ровно n (так как каждой вершине не хватает ровно 1),
        // но если где-то уже есть "лишняя" степень, сразу вернуть null.
        if (totalNeeded != n) 
            return null;

        // Попробуем "жадно" / бэктрекингом отобрать рёбра из chordCandidates.
        var usedInMatching = new bool[n]; // помечаем, у каких вершин уже добавлено нужное ребро
        var result = new List<(int,int)>();
        var maxChordLength = -1;
        
        // моя реализация поиска возможной хорды
        for (var i = 0; i < n; i++)
        {
            var possibleNeighborsList = possibleNeighbors[i].ToList();
            var checkedNeighbors = new HashSet<int>(); // помечаем, у каких вершин уже добавлено нужное ребро
            
            // если у вершины нет соседей, значит, нельзя построить хорду из этой вершины
            if (possibleNeighborsList.Count == 0)
                return null;

            // перемешиваем соседей
            Shuffle(possibleNeighborsList);
            
            while (!usedInMatching[i] && checkedNeighbors.Count < possibleNeighborsList.Count)
            {
                var randomIndex = rand.Next(possibleNeighborsList.Count);
                var selectedNeighbor = possibleNeighborsList[randomIndex];
                checkedNeighbors.Add(selectedNeighbor);

                if (!usedInMatching[selectedNeighbor])
                {
                   result.Add((i, selectedNeighbor));
                   usedInMatching[i] = true;
                   usedInMatching[selectedNeighbor] = true;

                   if (Math.Max(Math.Min(Math.Abs(i - selectedNeighbor), i - (selectedNeighbor - n)), maxChordLength) >
                       maxChordLength)
                   {
                       graph.MaxChord = (i, selectedNeighbor);
                   }
                   maxChordLength = Math.Max(Math.Min(Math.Abs(i - selectedNeighbor), i - (selectedNeighbor - n)), maxChordLength);
                   graph.MaxChordLength = maxChordLength;
                }
            }
            
            // если вершина так и осталась без пары, значит, построить для нее хорду не удалось, возвращаем null
            if (!usedInMatching[i])
            {
                return null;
            }
        }

        // foreach (var (u, v) in chordCandidates)
        // {
        //     // Если обе вершины ещё нуждаются в ребре
        //     if (!usedInMatching[u] && !usedInMatching[v])
        //     {
        //         // Добавим ребро в итоговое паросочетание
        //         result.Add((u, v));
        //         usedInMatching[u] = true;
        //         usedInMatching[v] = true;
        //     }
        // }

        // Проверяем, все ли вершины получили по одному ребру
        for (int i = 0; i < n; i++)
        {
            if (!usedInMatching[i]) 
                return null; // не все вершины покрыты
        }

        return result;
    }

    // Функция перемешивания списка (Fisher-Yates)
    private static void Shuffle<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = rand.Next(i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
}
