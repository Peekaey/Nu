using Nu_Models.DatabaseModels;

namespace Nu_DataService.Interfaces;

public interface ILibraryFolderIndexRepository
{
    Task AddAsync(LibraryFolderIndex fileIndex);
    void Add(LibraryFolderIndex fileIndex);
    Task AddRangeAsync(List<LibraryFolderIndex> fileIndexes);
    void AddRange(List<LibraryFolderIndex> folderIndexes);
    void Remove(LibraryFolderIndex fileIndex);
    void Update(LibraryFolderIndex fileIndex);
    Task<LibraryFolderIndex?> GetAsync(int id);
    LibraryFolderIndex? Get(int id);
    IEnumerable<LibraryFolderIndex> GetAll();
    LibraryFolderIndex? GetLibraryRootFolder();
    LibraryFolderIndex? GetLibraryFolderWithChildren(int id);
    IEnumerable<LibraryFolderIndex> GetFoldersByFolderName(List<string> folderNames);

}