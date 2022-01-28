using BenchmarkDotNet.Attributes;

namespace PerformanceApp
{
    public class Test
    {
        static CompiledSetter compiledSetter = new CompiledSetter();
        static ReflectionSetter reflectionSetter = new ReflectionSetter();
        static CachedReflectionSetter cachedReflectionSetter = new CachedReflectionSetter();

        static Entity[]? entities = null;

        [GlobalSetup]
        public static void Setup()
        {
            Console.WriteLine("Initializing CompiledSetter");
            compiledSetter.Initialize(typeof(Entity));
            cachedReflectionSetter.Initialize(typeof(Entity));

            Console.WriteLine("Initializing Entities");
            entities = Enumerable.Range(0, 200).Select(x => new Entity()).ToArray();
        }

        private static void Common(Setter setter)
        {
            for (var i = 0; i < entities?.Length; i++)
            {
                setter.Set(entities[i], nameof(Entity.Id), i);
                setter.Set(entities[i], nameof(Entity.Name), i.ToString());
            }
        }

        [Benchmark]
        public static void TestCompiled()
        {
            Common(compiledSetter);
        }

        [Benchmark]
        public static void TestReflection()
        {
            Common(reflectionSetter);
        }

        [Benchmark]
        public static void TestCachedReflection()
        {
            Common(cachedReflectionSetter);
        }
    }
}