using ThomasonAlgorithm;
using ThomasonAlgorithm.obj;

public class Algorithm
{
    private CubicGraph _graph;

    public Algorithm(CubicGraph graph)
    {
        _graph = graph;
    }

    public HashSet<int> FindSecondHamiltonianCycle()
    {
        ReconstructCycle();
        var newHamiltonianCycleSequence = GetHamiltonianCycleSequencedVertices();
        
        Console.WriteLine("Второй гамильтонов цикл найден");
        return newHamiltonianCycleSequence;
    }
    
    public void FindSecondHamiltonianCycleVoid()
    {
        ReconstructCycle();
        
        Console.WriteLine("Второй гамильтонов цикл найден");
    }

    private HashSet<int> GetHamiltonianCycleSequencedVertices()
    {
        var sequence = new HashSet<int>();
        var currentVertex = 0;

        while (!sequence.Contains(currentVertex))
        {
            sequence.Add(currentVertex);
            
            var currentVertexNeighborsInCycle = GetVertexNeighborsInCycle(currentVertex);
            var nextVertex = currentVertexNeighborsInCycle.FirstPossibleNeighborOrNull(x => !sequence.Contains(x));

            currentVertex = nextVertex ?? currentVertex;
        }
        
        return sequence;        
    }

    private Dictionary<int, List<int>> ReconstructCycle()
    {
        var hamiltonianCycle = _graph.HamiltonianCycle;
        var startVertex = 0;
        
        // разъединяем стартовую вершину и первого ее соседа
        var neighborToUnlink = hamiltonianCycle[startVertex][0];
        
        // только для дебага на своем графе
        // var neighborToUnlink = hamiltonianCycle[startVertex][1];
        // hamiltonianCycle[startVertex][1] = -1;
        
        hamiltonianCycle[startVertex][0] = -1;
        
        var startVertexIndex = hamiltonianCycle[neighborToUnlink].IndexOf(startVertex);
        hamiltonianCycle[neighborToUnlink][startVertexIndex] = -1;

        // среди всех соседей отсоединенной вершины находим ту, ребро с которой добавится в цикл 
        var nextVertex = GetNewEdgeEnd(startVertex, neighborToUnlink);

        while (nextVertex != startVertex)
        {
            // ищем ребро nextVertex, которое убираем из гам. цикла
            var nextNeighborToUnlink = GetNeighborToUnlink(nextVertex);
            // вершину, с которой разъединяем nextVertex заменяем на ту, из которой провели последнее ребро
            var unlinkVertexIndex = hamiltonianCycle[nextVertex].IndexOf(nextNeighborToUnlink);
            hamiltonianCycle[nextVertex][unlinkVertexIndex] = neighborToUnlink;
            
            // у отсоединенной вершины убираем из списка соседей nextVertex
            var neighborToUnlinkIndex = hamiltonianCycle[nextNeighborToUnlink].IndexOf(nextVertex);
            hamiltonianCycle[nextNeighborToUnlink][neighborToUnlinkIndex] = -1;
            
            nextVertex = GetNewEdgeEnd(nextVertex, nextNeighborToUnlink);
            neighborToUnlink = nextNeighborToUnlink;
        }

        var startIndexEmptyNeighbor = hamiltonianCycle[startVertex].IndexOf(-1);
        hamiltonianCycle[startVertex][startIndexEmptyNeighbor] = neighborToUnlink; 

        return hamiltonianCycle;
    }

    private List<int> GetVertexNeighbors(int v)
    {
        var neighbors = new List<int>();

        for (var i = 0; i < _graph.Graph.GetLength(0); ++i)
        {
            if (_graph.Graph[v, i] == 1)
                neighbors.Add(i);
        }
        
        return neighbors;
    }

    private List<int> GetVertexNeighborsInCycle(int v)
    {
        return _graph.HamiltonianCycle[v];
    }

    private int GetNewEdgeEnd(int previousNeib, int currentVertex)
    {
        var currentVertexNeighbors = GetVertexNeighbors(currentVertex);
        var currentVertexNeighborsInCycle = GetVertexNeighborsInCycle(currentVertex);
        var emptyIndex = currentVertexNeighborsInCycle.IndexOf(-1);
        
        var neighborToAdd = currentVertexNeighbors.First(x => !currentVertexNeighborsInCycle.Contains(x) && x != previousNeib);
        currentVertexNeighborsInCycle[emptyIndex] = neighborToAdd;

        return neighborToAdd;
    }

    private int GetNeighborToUnlink(int currentVertex)
    {
        var currentVertexNeighborsInCycle = GetVertexNeighborsInCycle(currentVertex);

        if (CheckCycle(currentVertex, currentVertexNeighborsInCycle.First()))
            return currentVertexNeighborsInCycle.First();
        
        return currentVertexNeighborsInCycle.Last();
    }

    private bool CheckCycle(int currentVertex, int neighbor)
    {
        var visited = new HashSet<int> { currentVertex };
        var observedVertex = neighbor;

        while (!visited.Contains(observedVertex))
        {
            visited.Add(observedVertex);
            
            var neighborsInCycle = GetVertexNeighborsInCycle(observedVertex);
            
            // если среди соседей в гам. цикле есть вершины, которые еще не посещены ИЛИ не пустые (!= -1) 
            if (neighborsInCycle.Any(x => !visited.Contains(x) && x != -1))
            {
                observedVertex = neighborsInCycle.First(x => !visited.Contains(x) && x != -1);
            } 
            else if (neighborsInCycle.Any(x => !visited.Contains(x)) && neighborsInCycle.Any(x => x == -1))
            {
                return false;
            }
        }

        return true;
    }
}