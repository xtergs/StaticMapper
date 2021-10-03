
using StaticMapper.ConsoleApp;
using StaticMapperGenerator.Implementation;
namespace StaticMapper.ConsoleApp
{
	partial class MapperAB : IMapper
	{
		public static B Map(A value)
		{
			var result = new  B();
			result.Property1 = value.Property1;
			return result;
		}
		object IMapper.Map(object value)
		{
			return Map(value as A);
		}
	}
}
