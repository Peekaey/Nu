using Nu_Models.DatabaseModels;

namespace Nu_DataService.Interfaces;

public interface ILibraryFileIndexRepository
{
    Task AddAsync(LibraryFileIndex fileIndex);
    void Add(LibraryFileIndex fileIndex);
    Task AddRangeAsync(List<LibraryFileIndex> fileIndexes);
    void AddRange(List<LibraryFileIndex> fileIndexes);
    void Remove(LibraryFileIndex fileIndex);
    void Update(LibraryFileIndex fileIndex);
    Task<LibraryFileIndex?> GetAsync(int id);
    LibraryFileIndex? Get(int id);
    IEnumerable<LibraryFileIndex> GetAll();
}