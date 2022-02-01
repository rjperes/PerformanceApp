using BenchmarkDotNet.Running;

namespace PerformanceApp;

public class Program
{
    static void Main(string[] args)
    {
        BenchmarkRunner.Run<Test>();
    }
}