using Nu_Models.DatabaseModels;

namespace Nu_DataService.Interfaces;

public interface ILibraryFolderIndexRepository
{
    Task<LibraryFolderIndex> AddAsync(LibraryFolderIndex fileIndex);
    void Add(LibraryFolderIndex fileIndex);
    Task<LibraryFolderIndex> RemoveAsync(LibraryFolderIndex fileIndex);
    Task<List<LibraryFolderIndex>> AddRangeAsync(List<LibraryFolderIndex> fileIndexes);
    void AddRange(List<LibraryFolderIndex> folderIndexes);
    void Remove(LibraryFolderIndex fileIndex);
    Task<LibraryFolderIndex> UpdateAsync(LibraryFolderIndex fileIndex);
    void Update(LibraryFolderIndex fileIndex);
    Task<LibraryFolderIndex> GetAsync(int id);
    LibraryFolderIndex Get(int id);
    List<LibraryFolderIndex> GetAll();
    

}