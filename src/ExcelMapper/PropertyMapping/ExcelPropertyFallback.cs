namespace ExcelMapper.PropertyMapping
{
    public abstract class ExcelPropertyFallback<TProperty>
    {
        public abstract TProperty Resolve();
    }
}
