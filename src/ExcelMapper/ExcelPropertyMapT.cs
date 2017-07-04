using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ExcelMapper.Pipeline;

namespace ExcelMapper
{
    public class ExcelPropertyMap<TDeclaringType, TProperty> : ExcelPropertyMap where TDeclaringType : new()
    {
        internal ExcelPropertyMap(MemberInfo member) : base(typeof(TDeclaringType), member)
        {
            var pipeline = new ColumnPipeline<TProperty>(member.Name);
            AutoMap(pipeline);
            Pipeline = pipeline;
        }

        public TPipeline WithPipeline<TPipeline>(TPipeline pipeline) where TPipeline : Pipeline.Pipeline
        {
            Pipeline = pipeline;
            return pipeline;
        }

        public ColumnPipeline<TProperty> WithColumnName(string columnName)
        {
            return WithPipeline(new ColumnPipeline<TProperty>(columnName));
        }

        public IndexPipeline<TProperty> WithIndex(int index)
        {
            return WithPipeline(new IndexPipeline<TProperty>(index));
        }

        private void AutoMap(Pipeline<TProperty> pipeline)
        {
            var pipelineItems = new List<PipelineItem<TProperty>>();

            Type type = typeof(TDeclaringType);
            Type[] interfaces = type.GetTypeInfo().ImplementedInterfaces.ToArray();

            if (type == typeof(DateTime))
            {
                var item = new ParseAsDateTime(null) as PipelineItem<TProperty>;
                pipelineItems.Add(item);
            }
            else if (interfaces.Any(t => t == typeof(IConvertible)))
            {
                var item = new ChangeType<TProperty>();
                pipelineItems.Add(item);

                // Allow empty strings, but don't allow empty or invalid primitives by default.
                if (type != typeof(string))
                {
                    var validationItem = new ThrowIfStatus<TProperty>(PipelineStatus.Empty | PipelineStatus.Invalid);
                    pipelineItems.Add(validationItem);
                }
            }
            else
            {
                Type ienumerableType = interfaces.FirstOrDefault(t => t.GetGenericTypeDefinition() == typeof(IEnumerable<>));
                if (ienumerableType != null)
                {
                    Type elementType = ienumerableType.GenericTypeArguments[0];
                    Type itemType = typeof(ParseWithDelimiter<>).MakeGenericType(elementType);
                    object item = Activator.CreateInstance(itemType);

                    // TODO
                }
            }

            Pipeline = pipeline.WithAdditionalItems(pipelineItems);
        }
    }
}
