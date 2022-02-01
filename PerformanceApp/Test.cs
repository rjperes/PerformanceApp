using BenchmarkDotNet.Attributes;

namespace PerformanceApp
{
    public class Test
    {
        static CompiledSetter compiledSetter = new CompiledSetter();
        static ReflectionSetter reflectionSetter = new ReflectionSetter();
        static CachedReflectionSetter cachedReflectionSetter = new CachedReflectionSetter();
        static FastMemberSetter fastMemberSetter = new FastMemberSetter();

        static Entity[]? entities = null;

        [GlobalSetup]
        public void Setup()
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
        public void TestCompiled() => Common(compiledSetter);

        [Benchmark(Baseline = true)]
        public void TestReflection() => Common(reflectionSetter);

        [Benchmark]
        public void TestCachedReflection() => Common(cachedReflectionSetter);        

        [Benchmark]
        public void TestFastMember() => Common(fastMemberSetter);        
    }
}