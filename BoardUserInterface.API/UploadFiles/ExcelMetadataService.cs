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

