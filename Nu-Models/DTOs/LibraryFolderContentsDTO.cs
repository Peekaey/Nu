namespace Nu_Models.DTOs;

public class LibraryFolderContentsDTO
{
    public List<LibraryImageDTO> Files { get; set; }
    public List<LibraryAlbumDTO> Folders { get; set; }
    public List<LibraryFolderPathChunk> FolderPath{ get; set; }
}