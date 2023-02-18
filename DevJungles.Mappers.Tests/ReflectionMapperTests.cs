using DevJungles.Mappers.ReflectionMapper;
using DevJungles.Mappers.Tests.Models;

namespace DevJungles.Mapper.Tests;

public class ReflectionMapperTests
{
    [Fact]
    public void SimpleMapping()
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

    [Fact]
    public void RecursiveMapping()
    {
        var person = new Person()
        {
            Name = "Ivan",
            Surname = "Ivanov",
            Address = new() { Street = "Kovalenko", House = 54, Flat = 2 }
        };

        var Mapper = new ReflectionMapper();
        var personDto = Mapper.Map<PersonDto>(person);

        Assert.Equal(person.Name, personDto.Name);
        Assert.Equal(person.Surname, personDto.Surname);

        Assert.Equal(person.Address.Street, personDto.Address?.Street);
        Assert.Equal(person.Address.House, personDto.Address?.House);
        Assert.Equal(person.Address.Flat, personDto.Address?.Flat);
    }
}