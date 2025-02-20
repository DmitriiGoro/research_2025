namespace ThomasonAlgorithm;

public class CubicGraph
{
    // public Dictionary<int, List<int>> graph;
    public int[,] Graph;
    public Dictionary<int, List<int>> HamiltonianCycle;
    
    public CubicGraph(int[,] graph, Dictionary<int, List<int>> hamiltonianCycle)
    {
        Graph = graph;
        HamiltonianCycle = hamiltonianCycle;
    }

    public void AddEdge(int from, int to)
    {
        if (GetVertexDegree(from) == 3 || GetVertexDegree(to) == 3)
            throw new Exception("Степень вершины не может быть > 3");
        
        Graph[from, to] = 1;
        Graph[to, from] = 1;
    }

    public int GetVertexDegree(int vertex)
    {
        var degree = 0;

        for (int i = 0; i < Graph.GetLength(0); i++)
        {
            degree += Graph[i, vertex];
        }

        return degree;
    }
}