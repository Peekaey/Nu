using Nu_Models.DatabaseModels;

namespace Nu_DataService.Interfaces;

public interface ILibraryFileService
{
    IEnumerable<LibraryFileIndex> GetAllLibraryFiles();
    LibraryFileIndex? GetLibraryFileById(int id);
}