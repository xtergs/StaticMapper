using System;
using Microsoft.CodeAnalysis;

namespace StaticMapperGenerator.Generator
{
    internal class CustomMapperDefinition
    {
        public CustomMapperDefinition(ISymbol classDefinition)
        {
            ClassDefinition = classDefinition ?? throw new ArgumentNullException(nameof(classDefinition));
        }

        public ISymbol ClassDefinition { get; }
    }
}