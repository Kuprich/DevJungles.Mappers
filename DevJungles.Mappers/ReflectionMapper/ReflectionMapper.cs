using System.Reflection;

namespace DevJungles.Mappers.ReflectionMapper;

public class ReflectionMapper : IMapper
{
    public TDest Map<TSource, TDest>(TSource source) where TDest : new()
    {
        if (source == null) return new TDest();

        return Map<TDest>(source);
    }

    public TDest Map<TDest>(object source) where TDest : new()
    {
        var sourceProps = source.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
        var destProps = typeof(TDest).GetProperties(BindingFlags.Instance | BindingFlags.Public);

        var result = new TDest();

        foreach (var destProp in destProps)
        {
            var sourceProp = sourceProps.FirstOrDefault(x => x.Name == destProp.Name);
            if (sourceProp != null && sourceProp.CanRead && destProp.CanWrite)
            {
                destProp.SetValue(result, sourceProp.GetValue(source));
            }
        }

        return result;
    }
}
