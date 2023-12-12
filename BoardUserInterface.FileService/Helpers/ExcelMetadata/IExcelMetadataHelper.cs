namespace BoardUserInterface.FileService.Helpers.ExcelMetadata;

public interface IExcelMetadataHelper
{
    string GetVersionNumberFromExcel(Stream excelFileStream);
}

