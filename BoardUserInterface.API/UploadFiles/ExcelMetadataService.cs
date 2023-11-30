using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.ExtendedProperties;
using DocumentFormat.OpenXml.CustomProperties;


namespace BoardUserInterface.API.UploadFiles;

public interface IExcelMetadataService
{
    string GetVersionNumberFromExcel(Stream excelFileStream);
}

public class ExcelMetadataService : IExcelMetadataService
{
    /*
    public string GetVersionNumberFromExcel(Stream excelFileStream)
    {


        using (var document = SpreadsheetDocument.Open(excelFileStream, false))
        {
            var extendedFilePropertiesPart = document.ExtendedFilePropertiesPart;

            if (extendedFilePropertiesPart != null)
            {
                // Load the properties
                var properties = extendedFilePropertiesPart.Properties.Elements<Properties>().FirstOrDefault();

                if (properties != null)
                {
                    // Access the properties, including the version
                    string version = properties.Application.ToString();
                    string title = properties.TitlesOfParts.ToString();
                }
            }

            var customProps = document.CustomFilePropertiesPart;
            if (customProps != null)
            {
                var props = customProps.Properties;
                if (props != null)
                {
                    // Assuming 'Version' is the property name for the version number in metadata
                    var versionProp = props.FirstOrDefault(p => p.XName == "Version");
                    if (versionProp != null)
                    {
                        return versionProp.InnerText;
                    }
                }
            }
        }
        return null; // Return null if no version number is found
    }
    */

    public string GetVersionNumberFromExcel(Stream excelFileStream)
    {
        using (var document = SpreadsheetDocument.Open(excelFileStream, false))
        {
            // Access Core File Properties Part
            var coreProps = document.PackageProperties;
            return coreProps.Version;

        }
    }
}

