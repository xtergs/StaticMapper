
using StaticMapper.ConsoleApp;
namespace StaticMapper.ConsoleApp
{
	partial class MyMapper
	{
		protected override void Init()
		{
			AddMap(typeof(A), typeof(B), new MapperAB());
		}
	}
}
