namespace ThomasonAlgorithm.Tools;

public static class SimpleCubicGraphGenerator
{
    public static int[,] Main()
    {
        int n = 10; // Количество вершин
        int[,] adjacencyMatrix = new int[n, n];

        // Инициализируем генератор случайных чисел
        Random random = new Random();

        // Добавляем рёбра для каждой вершины
        for (int i = 0; i < n; i++)
        {
            int edgesAdded = 0; // Счетчик добавленных рёбер для текущей вершины

            // Пока не добавлено 3 ребра
            while (edgesAdded < 3 && CountEdges(adjacencyMatrix, i) < 3)
            {
                // Выбираем случайную вершину j, с которой можно соединить i
                int j = random.Next(n);

                // Проверяем, что i и j не совпадают, и что ребро ещё не добавлено
                if (i == j || adjacencyMatrix[i, j] != 0 || CountEdges(adjacencyMatrix, j) >= 3) continue;
                // Добавляем ребро между i и j
                adjacencyMatrix[i, j] = 1;
                adjacencyMatrix[j, i] = 1;
                edgesAdded++;
            }
        }

        // Выводим первые 10 строк и столбцов матрицы для проверки
        // Console.WriteLine("Матрица смежности кубического графа на 100 вершинах (первые 10x10 элементов):");
        // for (int i = 0; i < n; i++)
        // {
        //     for (int j = 0; j < n; j++)
        //     {
        //         Console.Write(adjacencyMatrix[i, j] + " ");
        //     }
        //     Console.WriteLine();
        // }

        return adjacencyMatrix;
    }

    // Метод для подсчёта количества рёбер у вершины
    static int CountEdges(int[,] matrix, int vertex)
    {
        int count = 0;
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            if (matrix[vertex, i] == 1)
            {
                count++;
            }
        }
        return count;
    }
}
