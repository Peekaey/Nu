using Nu_Models.Enums;

namespace Nu_Models.DTOs;

public class FolderDTO
{
    public string FolderName { get; set; }
    public string FolderPath { get; set; }
    public FolderLevel FolderLevel { get; set; }
    public FolderDTO? ParentFolder { get; set; }

}