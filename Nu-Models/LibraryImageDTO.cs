namespace Nu_Models;

public class LibraryImageDTO
{
    public int Id { get; set; }
    public string FileName { get; set; }
    public string FilePath { get; set; }
    public string FileType { get; set; }
    public double FileSizeMb { get; set; }
    public DateTime SystemCreationTime { get; set; }
    // Base 64 for now - optimise later
    public string ImageData { get; set; }
}