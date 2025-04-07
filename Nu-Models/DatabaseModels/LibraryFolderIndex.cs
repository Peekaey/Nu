using System.ComponentModel.DataAnnotations.Schema;

namespace Nu_Models.DatabaseModels;

public class LibraryFolderIndex : IAuditable
{
    public int Id { get; set; }
    public string FolderName { get; set; }
    public string FolderPath { get; set; }
    public int FolderLevel { get; set; }
    public int? ParentFolderId { get; set; }
    [ForeignKey("ParentFolderId")]
    public LibraryFolderIndex ParentFolder { get; set; }
    public ICollection<LibraryFolderIndex> ChildFolders { get; set; } = new List<LibraryFolderIndex>();
    
    // Navigation property for files in this folder
    public ICollection<LibraryFileIndex> LibraryFiles { get; set; } = new List<LibraryFileIndex>();
    
    public DateTime CreatedDate { get; set; }
    public DateTime LastUpdatedDate { get; set; }
}