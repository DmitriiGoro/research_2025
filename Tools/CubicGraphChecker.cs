namespace ThomasonAlgorithm.Tools;

/// <summary>
/// алгоритм для проверки корректности матрицы-смежности кубического графа
/// </summary>
public static class CubicGraphChecker
{
    public static bool CheckGraph(int[,] graph)
    {
        for (int i = 0; i < graph.GetLength(0); i++)
        {
            var degree = 0;
            for (int j = 0; j < graph.GetLength(1); j++)
            {
                degree += graph[i, j];
            }

            if (degree != 3)
            {
                Console.WriteLine($"Граф не кубический, проблема в {i}-ой строке");
                return false;
            }

        }
        
        for (int i = 0; i < graph.GetLength(0); i++)
        {
            for (int j = 0; j < graph.GetLength(1); j++)
            {
                if (graph[i, j] == 1)
                {
                    if (graph[j, i] != 1)
                    {
                        Console.WriteLine($"Матрица смежности построена неверно, проблема в паре {i}-{j}");
                        return false;
                    }
                }
            }

        }
        
        Console.WriteLine("Граф построен корректно");
        return true;
    }
}