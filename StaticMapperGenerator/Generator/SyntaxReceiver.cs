using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using StaticMapperGenerator.Helpers;
using StaticMapperGenerator.Implementation;

namespace StaticMapperGenerator.Generator
{
    internal class SyntaxReceiver : ISyntaxContextReceiver
    {
        public List<string> Log { get; } = new();
        
        public List<WorkItem> WorkItems { get; } = new();

        public CustomMapperDefinition MapperClass { get; private set; }

        public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
        {
            try
            {
                if (context.Node is InvocationExpressionSyntax classDeclarationSyntax)
                {
                    Log.Add($"Found a class named:{classDeclarationSyntax.ToString()}");
                    var declared = context.SemanticModel.GetSymbolInfo(context.Node);


                    var methodSymbol = declared.Symbol as IMethodSymbol;
                    if (methodSymbol == null) return;

                    Log.Add($"Found a class named2:{methodSymbol.Name}");
                    if (methodSymbol.Arity <= 0) return;

                    Log.Add("Arity > 0");

                    foreach (var x in methodSymbol.TypeArguments.OfType<INamedTypeSymbol>()) Log.Add($"{x.FullName()}");


                    WorkItems.Add(
                        new WorkItem(declared.Symbol, methodSymbol.TypeArguments[0], methodSymbol.TypeArguments[1]));
                }


                if (context.Node is ClassDeclarationSyntax)
                {
                    var testClass = (INamedTypeSymbol)context.SemanticModel.GetDeclaredSymbol(context.Node)!;


                    Log.Add($"Found a class named:{testClass!.Name}");
                    if (testClass.BaseType.Name != nameof(StaticMapper)) return;

                    MapperClass = new CustomMapperDefinition(testClass);
                }
            }
            catch (Exception ex)
            {
                Log.Add("Error parsing sytax " + ex);
            }
        }
    }
}