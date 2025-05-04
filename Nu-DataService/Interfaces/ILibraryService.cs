using Nu_Models.DatabaseModels;
using Nu_Models.DTOs;

namespace Nu_DataService.Interfaces;

public interface ILibraryService
{
    LibraryFolderIndex GetLibraryFolderWithChildren(int id);
    List<LibraryFolderIndex> GetAllLibraryFolders();
    IList<LibraryFileIndex> GetAllLibraryFiles();
    LibraryFolderIndex? GetLibraryRootContents();
    LibraryFileIndex GetLibraryFileById(int id);
    List<LibraryFolderPathChunk> GetLibraryFolderPathChunkIds(List<LibraryFolderPathChunk> folderPathChunks);
}