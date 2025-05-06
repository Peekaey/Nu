using Nu_Models.DatabaseModels;
using Nu_Models.DTOs;

namespace Nu_DataService.Interfaces;

public interface ILibraryFolderService
{
    LibraryFolderIndex? GetLibraryRootContents();
    LibraryFolderIndex? GetLibraryFolderWithChildren(int id);
    IEnumerable<LibraryFolderIndex> GetAllLibraryFolders();
    IEnumerable<LibraryFolderPathChunk> GetLibraryFolderPathChunkIds(List<LibraryFolderPathChunk> folderPathChunks);
}