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
        return (TDest)Map(typeof(TDest), source);
    }

    private object Map(Type destType, object source)
    {
        var sourceProps = source?.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
        var destProps = destType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
        var result = Activator.CreateInstance(destType);

        foreach (var destProp in destProps)
        {
            var sourceProp = sourceProps?.FirstOrDefault(x => x.Name == destProp.Name);
            if (sourceProp != null && sourceProp.CanRead && destProp.CanWrite)
            {
                object? sourcePropValue = sourceProp.GetValue(source);

                if (sourcePropValue != null && !sourcePropValue.GetType().IsAssignableTo(destProp.PropertyType))
                {
                    sourcePropValue = Map(destProp.PropertyType, sourcePropValue);
                }

                destProp.SetValue(result, sourcePropValue);

            }
        }

        return result ?? new object();
    }
}
