using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace BoardUserInterface.API.SwaggerOptions
{
    /// <summary>
    /// Create Swagger document filter that makes all endpoints in lowercase
    /// </summary>
    public class LowercaseDocumentFilter : IDocumentFilter
    {
        /// <inheritdoc/>
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var paths = swaggerDoc.Paths.ToDictionary(
                entry => entry.Key.ToLowerInvariant(),
                entry => entry.Value
            );

            swaggerDoc.Paths.Clear();
            foreach (var pathItem in paths)
            {
                swaggerDoc.Paths.Add(pathItem.Key, pathItem.Value);
            }
        }
    }
}
