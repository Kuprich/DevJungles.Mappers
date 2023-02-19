using System.Linq.Expressions;

namespace DevJungles.Mappers.ReflectionMapper;

public record MapperConfiguration(IEnumerable<TypeMappingConfiguration> TypeMappings);

public record TypeMappingConfiguration(Type SourceType, Type DestType, IEnumerable<MemberMappingConfiguration> MemberMappings);
public record MemberMappingConfiguration(string MemberName, IMappingAction Action);
public interface IMappingAction { }

public record IgnoreMappingAction : IMappingAction { }
public record MapAction : IMappingAction { }

public class MemberMappingConfigurationBuilder<TSource, TMember>
{
    private Func<TSource, TMember> _mapAction;
    private bool _ignore = false;
    public void Ignore()
    {
        _ignore = true;
    }
    public void Map(Func<TSource, TMember> mapAction)
    {
        _mapAction = mapAction;
    }

}

public class TypeMappingBuilder<TSource, TDest>
{
    public TypeMappingBuilder<TSource, TDest> ForMember<TProp>(string destMemberName, Action<MemberMappingConfigurationBuilder<TSource, TProp>> memberOptions) => this;
    public TypeMappingBuilder<TSource, TDest> ForMember<TProp>(Expression<Func<TDest, TProp>> destMember, Action<MemberMappingConfigurationBuilder<TSource, TProp>> memberOptions) => this;
}

public class MapperConfigurationBuilder
{
    public TypeMappingBuilder<TSource, TDest> CreateMap<TSource, TDest>()
    {
        return new TypeMappingBuilder<TSource, TDest>();
    }
}