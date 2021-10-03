using System;
using Microsoft.CodeAnalysis;

namespace StaticMapperGenerator.Generator
{
    internal class WorkItem
    {
        public WorkItem(ISymbol testClass, ITypeSymbol sourceType, ITypeSymbol destinationType)
        {
            TestClass = testClass ?? throw new ArgumentNullException(nameof(testClass));
            SourceType = sourceType ?? throw new ArgumentNullException(nameof(sourceType));
            DestinationType = destinationType ?? throw new ArgumentNullException(nameof(destinationType));
        }

        public ISymbol TestClass { get; }
        
        public ITypeSymbol SourceType { get; }
        
        public ITypeSymbol DestinationType { get; }
    }
}