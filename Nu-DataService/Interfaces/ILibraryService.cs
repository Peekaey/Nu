using Nu_Models.DatabaseModels;

namespace Nu_DataService.Interfaces;

public interface ILibraryService
{
    LibraryFolderIndex GetLibraryFolderWithChildren(int id);
    List<LibraryFolderIndex> GetAllLibraryFolders();
    LibraryFolderIndex? GetLibraryRootContents();
}