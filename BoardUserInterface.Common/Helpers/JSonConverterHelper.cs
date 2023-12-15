using Newtonsoft.Json;

namespace BoardUserInterface.Helpers;

public static class JsonConverterHelper
{
    // Generic method to serialize an object to a JSON string
    public static string SerializeObject<T>(T obj)
    {
        // Check if the object is already a string
        if (obj is string str)
        {
            // Optionally, you could further check if the string is already in JSON format
            if (JsonHelper.IsValidJson(str))
            {
                return str;
            }
            else
            {
                // The string is not JSON, so it should be escaped and serialized
                return JsonConvert.SerializeObject(str);
            }
        }
        else
        {
            try
            {
                return JsonConvert.SerializeObject(obj);
            }
            catch (Exception ex)
            {
                // Handle or log the serialization exception as needed
                throw new InvalidOperationException("An error occurred during serialization.", ex);
            }
        }
    }



    // Generic method to deserialize a JSON string to an object of type T
    public static T Deserialize<T>(string json)
    {
        // Optionally, you could further check if the string is already in JSON format
        if (JsonHelper.IsValidJson(json))
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception ex)
            {
                // Handle or log the deserialization exception as needed
                throw new InvalidOperationException("An error occurred during deserialization.", ex);
            }
        }

        throw new InvalidOperationException("CHANGE");

        
    }
}
