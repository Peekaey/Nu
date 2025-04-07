using Nu_Models.DatabaseModels;

namespace Nu_DataService.Interfaces;

public interface ILibraryFileIndexRepository
{
    Task<LibraryFileIndex> AddAsync(LibraryFileIndex fileIndex);
    void Add(LibraryFileIndex fileIndex);
    Task<List<LibraryFileIndex>> AddRangeAsync(List<LibraryFileIndex> fileIndexes);
    Task<LibraryFileIndex> RemoveAsync(LibraryFileIndex fileIndex);
    void AddRange(List<LibraryFileIndex> fileIndexes);
    void Remove(LibraryFileIndex fileIndex);
    Task<LibraryFileIndex> UpdateAsync(LibraryFileIndex fileIndex);
    void Update(LibraryFileIndex fileIndex);
    Task<LibraryFileIndex> GetAsync(int id);
    LibraryFileIndex Get(int id);
}