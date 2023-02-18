using DevJungles.Mappers.ReflectionMapper;
using DevJungles.Mappers.Tests.Models;

namespace DevJungles.Mapper.Tests;

public class ReflectionMapperTests
{
    [Fact]
    public void ReflectionMapper_SimpleMapping()
    {
        var person = new Person()
        {
            Name = "Ivan",
            Surname = "Ivanov"
        };

        var Mapper = new ReflectionMapper();
        var personDto = Mapper.Map<PersonDto>(person);

        Assert.Equal(person.Name, personDto.Name);
        Assert.Equal(person.Surname, personDto.Surname);
    }
}