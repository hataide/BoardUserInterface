namespace BoardUserInterfaces.DataAccess.DTOs;

public class FilesDto
{
    public string Version { get; set; }
    public string Name { get; set; }
    public string Extension { get; set; }
    public byte[]? Content { get; set; }
}
