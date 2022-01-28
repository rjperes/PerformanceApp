using BenchmarkDotNet.Running;

namespace PerformanceApp;

public class Program
{
    static void Main(string[] args)
    {
        var getter = new CompiledGetter();
        getter.Initialize(typeof(Entity));

        var entity = new Entity();
        entity.Name = "Test";


        var name = getter.Get(entity, "Name");

        var setter = new CompiledSetter();
        setter.Initialize(typeof(Entity));

        setter.Set(entity, "Name", "Ricardo");


        //BenchmarkRunner.Run<Test>();
    }
}