using System.ComponentModel.DataAnnotations.Schema;
using Nu_Models.Enums;

namespace Nu_Models.DatabaseModels;

public class LibraryFileIndex : IAuditable
{
    public int Id { get; set; }
    public DateTime SystemCreationTime { get; set; }
    public string DirectoryName { get; set; }
    public string FileType { get; set; }
    public string FullName { get; set; }
    public string FileName { get; set; }
    public double FileSizeInMB { get; set; }
    
    public int? ParentFolderId { get; set; }
    
    // Navigation property for the parent folder
    [ForeignKey("ParentFolderId")]
    public LibraryFolderIndex ParentFolder { get; set; }
    
    public DateTime CreatedDate { get; set; }
    public DateTime LastUpdatedDate { get; set; }
}