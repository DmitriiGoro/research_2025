namespace ThomasonAlgorithm.Tools;

public class HamiltonianCycle
{
    // Проверка, можно ли добавить вершину в путь
    static bool IsSafe(int v, int[,] graph, int[] path, int pos)
    {
        // Вершина должна быть еще не в пути
        for (int i = 0; i < pos; i++)
        {
            if (path[i] == v)
            {
                return false;
            }
        }

        // Вершина должна быть соединена с предыдущей в пути
        if (graph[path[pos - 1], v] == 0)
        {
            return false;
        }

        return true;
    }

    // Рекурсивная функция для поиска гамильтонова цикла
    static bool FindHamiltonianCycle(int[,] graph, int[] path, int pos)
    {
        // Если все вершины в пути, проверяем, есть ли ребро от последней вершины к первой
        if (pos == graph.GetLength(0))
        {
            if (graph[path[pos - 1], path[0]] == 1)
            {
                return true;
            }
            return false;
        }

        // Пробуем добавить вершины в путь
        for (int v = 1; v < graph.GetLength(0); v++)
        {
            if (IsSafe(v, graph, path, pos))
            {
                path[pos] = v;

                // Рекурсивно пробуем продолжить добавление
                if (FindHamiltonianCycle(graph, path, pos + 1))
                {
                    return true;
                }

                // Откатываемся, если не получилось
                path[pos] = -1;
            }
        }

        return false;
    }

    // Основная функция, ищет гамильтонов цикл
    static Dictionary<int, List<int>> HamiltonianCycleExists(int[,] graph)
    {
        int n = graph.GetLength(0);
        int[] path = new int[n];
        for (int i = 0; i < n; i++)
        {
            path[i] = -1;
        }

        // Начинаем с вершины 0
        path[0] = 0;
        var cycleEdges = Enumerable
            .Range(0, n)
            .ToDictionary(i => i, i => new List<int>());

        if (FindHamiltonianCycle(graph, path, 1))
        {
            Console.WriteLine("Гамильтонов цикл найден");
            // добавляем в словарь ребер цикла вершину из 0-ой вершины в последнюю и наоборот
            cycleEdges[path[0]].Add(path[^1]);
            cycleEdges[path[^1]].Add(path[0]);

            for (int i = 1; i < n; i++)
            {
                cycleEdges[path[i]].Add(path[i - 1]);
                cycleEdges[path[i - 1]].Add(path[i]);
                // Console.Write(path[i] + " ");
                // Console.WriteLine($"вершина {path[i]} имеет соседей {path[i]} и {path[i - 1]} ");
            }
            
            return cycleEdges;
        }

        Console.WriteLine("Гамильтонов цикл не существует.");
        return [];

    }

    public static Dictionary<int, List<int>> GetHamiltonianCycle(int[,] graph)
    {
        var cycle = HamiltonianCycleExists(graph);

        return cycle;
    }
}
