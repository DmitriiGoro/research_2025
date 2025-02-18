namespace ThomasonAlgorithm.Models;

public class Experiment
{
    public Experiment(int verticesNumber, int k, int maxChordLength, double timeToFindFirstCycle, double timeToFindSecondCycle)
    {
        VerticesNumber = verticesNumber;
        K = k;
        MaxChordLength = maxChordLength;
        TimeToFindFirstCycle = timeToFindFirstCycle;
        TimeToFindSecondCycle = timeToFindSecondCycle;
    }
    
    public Guid Id { get; set; } = Guid.NewGuid();
    
    /// <summary>
    /// количество вершин графа
    /// </summary>
    public int VerticesNumber { get; set; }
    
    /// <summary>
    /// максимальнодопустимая длина хорды в графе
    /// </summary>
    public int K { get; set; }
    
    /// <summary>
    /// фактически максимальная длина хорды
    /// </summary>
    public int MaxChordLength { get; set; }
    
    /// <summary>
    /// время поиска первого гамильтонова цикла
    /// </summary>
    public double TimeToFindFirstCycle { get; set; }
    
    /// <summary>
    /// время поиска второго гамильтонова цикла
    /// </summary>
    public double TimeToFindSecondCycle { get; set; }
}