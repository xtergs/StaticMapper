using System.Linq;
using StaticMapperGenerator.Helpers;
using StaticMapperGenerator.Implementation;

namespace StaticMapperGenerator.Generator
{
    internal class MapClassGenerator
    {
        public static MapClassInfo GetMapClass(WorkItem workItem, string @namespace = null)
        {
            var info = new MapClassInfo
            {
                Name = $"Mapper{workItem.SourceType.Name}{workItem.DestinationType.Name}",
                Namespace = @namespace ?? workItem.DestinationType.FullNamespace(),
                SourceClassName = workItem.SourceType.Name,
                DestinationClassName = workItem.DestinationType.Name
            };

            var code = new CodeWriter();

            code.AppendLine();

            AddNamespaces(code,
                workItem.SourceType.FullNamespace(),
                workItem.DestinationType.FullNamespace(),
                typeof(IMapper).Namespace);

            using (code.BeginScope(
                $"namespace {@namespace ?? workItem.DestinationType.FullNamespace()}"))
            {
                using (code.BeginScope($"partial class {info.Name} : {nameof(IMapper)}"))
                {
                    MapMethod(code, workItem);
                    CommonMapMethod(code, workItem);
                }
            }

            info.Code = code.ToString();

            return info;
        }

        private static void AddNamespaces(CodeWriter code, params string[] namespaces)
        {
            foreach (var @namespace in namespaces.Distinct()) code.AppendLine($"using {@namespace};");
        }

        private static void MapMethod(CodeWriter code, WorkItem workItem)
        {
            using (code.BeginScope(
                $"public static {workItem.DestinationType.Name} Map({workItem.SourceType.Name} value)"))
            {
                code.AppendLine($"var result = new  {workItem.DestinationType.Name}();");
                var sourceProperties = workItem.SourceType.ReadWriteScalarProperties();
                var destinationProperties = workItem.DestinationType.ReadWriteScalarProperties();

                foreach (var property in destinationProperties)
                {
                    var source = sourceProperties.SingleOrDefault(x => x.Name == property.Name);
                    if (source == null) continue;

                    code.AppendLine($"result.{property.Name} = value.{property.Name};");
                }

                code.AppendLine("return result;");
            }
        }

        private static void CommonMapMethod(CodeWriter code, WorkItem workItem)
        {
            using (code.BeginScope(
                $"object {nameof(IMapper)}.Map(object value)"))
            {
                code.AppendLine($"return Map(value as {workItem.SourceType.Name});");
            }
        }
    }
}