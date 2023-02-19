using System.Linq.Expressions;

namespace DevJungles.Mappers.ReflectionMapper;

public record MapperConfiguration(IEnumerable<TypeMappingConfiguration> TypeMappings);
public record TypeMappingConfiguration(Type SourceType, Type DestType, IEnumerable<MemberMappingConfiguration> MemberMappings);
public record MemberMappingConfiguration(string MemberName, IMappingAction Action);
public interface IMappingAction { }
public record IgnoreMapAction : IMappingAction { }
public record MapAction(Delegate Action) : IMappingAction { }

public interface IMemberMappingConfigurationBuilder
{
    MemberMappingConfiguration Build();
}

public class MemberMappingConfigurationBuilder<TSource, TMember> : IMemberMappingConfigurationBuilder
{
    private Func<TSource, TMember> _mapAction;
    private bool _ignore = false;
    private readonly string _memberName;

    public void Ignore() => _ignore = true;
    public void Map(Func<TSource, TMember> mapAction) => _mapAction = mapAction;

#pragma warning disable CS8618
    public MemberMappingConfigurationBuilder(string memberName) => _memberName = memberName;
#pragma warning restore

    public MemberMappingConfiguration Build()
    {
        return new(_memberName, _ignore ? new IgnoreMapAction() : new MapAction(_mapAction));
    }


}

public interface ITypeMappingConfigurationBuilder
{
    TypeMappingConfiguration Build();
}

public class TypeMappingConfigurationBuilder<TSource, TDest> : ITypeMappingConfigurationBuilder
{
    private readonly List<IMemberMappingConfigurationBuilder> _builders;
    public TypeMappingConfigurationBuilder<TSource, TDest> ForMember<TProp>(string destMemberName, Action<MemberMappingConfigurationBuilder<TSource, TProp>> memberOptions)
    {
        var builder = new MemberMappingConfigurationBuilder<TSource, TProp>(destMemberName);
        _builders.Add(builder);

        memberOptions(builder);
        return this;
    }
    public TypeMappingConfigurationBuilder<TSource, TDest> ForMember<TProp>(Expression<Func<TDest, TProp>> destMember, Action<MemberMappingConfigurationBuilder<TSource, TProp>> memberOptions)
    {
        var memberName = ((MemberExpression)destMember.Body).Member.Name;
        return ForMember(memberName, memberOptions);
    }

    public TypeMappingConfiguration Build()
    {
        return new TypeMappingConfiguration(typeof(TSource), typeof(TDest), _builders.Select(x => x.Build()));
    }

}

public class MapperConfigurationBuilder
{
    private readonly List<ITypeMappingConfigurationBuilder> _builders;
    public TypeMappingConfigurationBuilder<TSource, TDest> CreateMap<TSource, TDest>()
    {
        var builder = new TypeMappingConfigurationBuilder<TSource, TDest>();
        _builders.Add(builder);
        return builder;
    }

    public MapperConfiguration Build()
    {
        return new MapperConfiguration(_builders.Select(x => x.Build()));
    }
}