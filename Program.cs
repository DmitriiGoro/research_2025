using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ThomasonAlgorithm;
using ThomasonAlgorithm.Models;
using ThomasonAlgorithm.Tools;

// ниже блок, в котором генерируется случайный кубический граф. Далее он валидируется, после чего в нем находится 1-ый гамильтонов цикл
// затем идет поиск второго гамильтонова цикла с помощью алгоритма Томассона

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureServices((_, services) =>
    {
            var DbConnectionString = "Host=localhost;Port=5432;Username=postgres;Password=postgres;Database=postgres";

            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(DbConnectionString)); // Используйте строку подключения для SQL Server или PostgreSQL
        });

var app = builder.Build();

const int attempts = 5;
const int maxN = 1000;
const int maxK = 499;
var stopwatch = new Stopwatch();

for (var k = 2; k < maxK; k++)
    for (var n = k * 2; n < maxN; n += 2)
    {
        // строим случайный кубический граф на n вершинах с максимальной длиной хорды k
        var randomCubicGraph = CubicGraphGenerator.Generate(n, k);
        
        // если граф построить не удалось, то идем дальше
        if (randomCubicGraph == null)
        {
            Console.WriteLine("Не удалось сгенерировать кубический граф с заданными параметрами.");
            continue;
        }
        
        Console.WriteLine($"Сгенерирован 3-регулярный граф на {n} вершинах с k={k}, максимальная длина хорды = {randomCubicGraph.MaxChordLength}");
        
        var graph = randomCubicGraph.GetAdjacencyMatrix();
        
        // проверяем, что граф кубический и его матрица смежности построена корректно
        if (!CubicGraphChecker.CheckGraph(graph))
        {
            continue;
        } 
        
        // измеряем время поиска первого гамильтонова цикла
        stopwatch.Start();
        var cycleEdges = HamiltonianCycle.GetHamiltonianCycle(graph);
        stopwatch.Stop();
        var timeToFindFirstCycle = stopwatch.ElapsedMilliseconds;
        Console.WriteLine($"время поиска 1-го цикла: {timeToFindFirstCycle} млс");
        stopwatch.Reset();
        
        var cubicGraph = new CubicGraph(graph, cycleEdges);
        var thomasonAlgorithm = new Algorithm(cubicGraph);
        
        // измеряем время поиска второго гамильтонова цикла
        stopwatch.Start();
        thomasonAlgorithm.FindSecondHamiltonianCycleVoid();
        stopwatch.Stop();
        var timeToFindSecondCycle = stopwatch.ElapsedMilliseconds;
        Console.WriteLine($"время поиска 2-го цикла: {timeToFindSecondCycle} млс");
        stopwatch.Reset();

        Console.WriteLine();

        var experiment = new Experiment(n, k, randomCubicGraph.MaxChordLength, timeToFindFirstCycle, timeToFindSecondCycle);
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        // Добавляем объект в DbSet
        dbContext.Experiments.Add(experiment);

        // Сохраняем изменения в базе данных
        await dbContext.SaveChangesAsync();
    }
