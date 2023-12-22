using BoardUserInterface.Helpers;

namespace BoardUserInterface.ConsoleTest;

class Program
{
    static void Main(string[] args)
    {
        // Create a test object
        var person = new Person
        {
            Name = "John Doe",
            Age = 30
        };

        // Serialize the object to JSON
        string json = JsonConverterHelper.SerializeObject(person);
        Console.WriteLine("Serialized JSON:");
        Console.WriteLine(json);

        // Deserialize the JSON back to an object
        var deserializedPerson = JsonConverterHelper.Deserialize<Person>(json);
        Console.WriteLine("Deserialized object:");
        Console.WriteLine($"Name: {deserializedPerson.Name}, Age: {deserializedPerson.Age}");
    }
}

// Test class to serialize and deserialize
public class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
}
