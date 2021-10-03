using System;
using StaticMapperGenerator;

namespace StaticMapper.ConsoleApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            MapperBuilder.CreateMap<A, B>();
        }
    }

    public class A
    {
        public string Property1 { get; set; }
    }

    public class B
    {
        public string Property1 { get; set; }
    }
}