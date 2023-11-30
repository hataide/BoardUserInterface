namespace BoardUserInterface.API.Utils.Helpers;

public static class FileNameHelper
{ 
    public static string SetNewVersionFileName(string fileName, string uploadedFileVersion)
    {
        var nome = fileName.Split(".");
        return string.Join(".", nome, 0, nome.Length - 1) + "_v_" + uploadedFileVersion.Replace('.', '_') + "." + nome[nome.Length - 1];
    }
}
