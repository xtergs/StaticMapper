using System;
using StaticMapper.ConsoleApp;
using Xunit;

namespace StaticMapper.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void GeneratedMapper_OK()
        {
            // Arrange
            var a = new A
            {
                Property1 = Guid.NewGuid().ToString()
            };
            var mapper = new MyMapper();

            // Act
            var result = mapper.Map<B>(a);

            // Assert
            Assert.Equal(a.Property1, result.Property1);
        }
    }
}