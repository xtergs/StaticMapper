using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using StaticMapperGenerator.Helpers;

namespace StaticMapperGenerator.Generator
{
    [Generator]
    public class MapGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
        }

        public void Execute(GeneratorExecutionContext context)
        {
            if (!(context.SyntaxContextReceiver is SyntaxReceiver receiver)) return;

            context.AddSource("Logs",
                SourceText.From(
                    $@"/*{Environment.NewLine + string.Join(Environment.NewLine, receiver.Log) + Environment.NewLine}*/",
                    Encoding.UTF8)
            );

            List<MapClassInfo> mapClasses = new();

            foreach (var workItem in receiver.WorkItems)
            {
                var info = MapClassGenerator.GetMapClass(workItem,
                    receiver.MapperClass?.ClassDefinition.FullNamespace());
                mapClasses.Add(info);

                context.AddSource(info.Name + ".cs", SourceText.From(info.Code, Encoding.UTF8));
            }

            if (receiver.MapperClass != null)
            {
                var className = receiver.MapperClass.ClassDefinition.Name;
                var fileName = className + ".cs";
                var code = new CodeWriter();

                code.AppendLine();

                code.AppendLine($"using {receiver.MapperClass.ClassDefinition.FullNamespace()};");

                using (code.BeginScope($"namespace {receiver.MapperClass.ClassDefinition.FullNamespace()}"))
                {
                    using (code.BeginScope($"partial class {className}"))
                    {
                        using (code.BeginScope(
                            "protected override void Init()"))
                        {
                            foreach (var classInfo in mapClasses)
                                code.AppendLine(
                                    $"AddMap(typeof({classInfo.SourceClassName}), typeof({classInfo.DestinationClassName}), new {classInfo.Name}());");
                        }
                    }
                }

                context.AddSource(fileName, SourceText.From(code.ToString(), Encoding.UTF8));
            }
        }
    }
}