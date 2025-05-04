namespace Nu_Models.DatabaseModels;

public class LibraryPreviewThumbnailIndex : IAuditable
{
    public int Id { get; set; }
    public DateTime SystemCreationTime { get; set; }
    public string DirectoryName { get; set; }
    public string FileType { get; set; }
    public string FullName { get; set; }
    public string FileName { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime LastUpdatedDate { get; set; }
    
    public int LibraryFileIndexId { get; set; }
    public LibraryFileIndex LibraryFileIndex { get; set; }
    
}