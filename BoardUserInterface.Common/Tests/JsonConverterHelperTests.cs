using BoardUserInterface.Helpers;
using Xunit; // Using xUnit for example

namespace BoardUserInterface.Tests;

public class JsonConverterHelperTests
{
    [Fact]
    public void SerializeObject_Person_ReturnsValidJson()
    {
        // Arrange
        var person = new Person { Name = "John Doe", Age = 30 };

        // Act
        var json = JsonConverterHelper.SerializeObject(person);

        // Assert
        Assert.Equal("{\"Name\":\"John Doe\",\"Age\":30}", json);
    }

    [Fact]
    public void DeserializeObject_ValidJson_ReturnsPersonObject()
    {
        // Arrange
        var json = "{\"Name\":\"Jane Doe\",\"Age\":25}";

        // Act
        var person = JsonConverterHelper.Deserialize<Person>(json);

        // Assert
        Assert.NotNull(person);
        Assert.Equal("Jane Doe", person.Name);
        Assert.Equal(25, person.Age);
    }


    public class SimpleTests
    {
        [Fact]
        public void PassingTest()
        {
            Assert.Equal(4, 2 + 2);
        }
    }

}
