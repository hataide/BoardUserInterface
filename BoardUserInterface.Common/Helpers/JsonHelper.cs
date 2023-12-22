using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BoardUserInterface.Helpers;

public static class JsonHelper
{
    // Method to check if a string is valid JSON
    public static bool IsValidJson(string input)
    {
        input = input.Trim();
        if ((input.StartsWith("{") && input.EndsWith("}")) || // For object
        (input.StartsWith("[") && input.EndsWith("]")))   // For array
        {
            try
            {
                var obj = JToken.Parse(input);
                return true;
            }
            catch (JsonReaderException)
            {
                // Exception caught means the string is not valid JSON
                return false;
            }
        }
        return false;
    }
}
