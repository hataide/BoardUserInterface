using DocumentFormat.OpenXml.Packaging;


namespace BoardUserInterface.FileService.Helpers.ExcelMetadata;

public class ExcelMetadataHelper : IExcelMetadataHelper
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

