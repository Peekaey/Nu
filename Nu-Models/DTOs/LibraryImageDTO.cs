namespace Nu_Models.DTOs;

public class LibraryImageDTO
{
    public int Id { get; set; }
    public string FileName { get; set; }
    public string FilePath { get; set; }
    public string FileType { get; set; }
    public double FileSizeMb { get; set; }
    public DateTime SystemCreationTime { get; set; }
    public string ServerImagePath { get; set; }
    
    public int? PreviewThumbNailId { get; set; }
}