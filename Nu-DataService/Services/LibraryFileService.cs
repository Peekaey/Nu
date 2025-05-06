using Microsoft.Extensions.Logging;
using Nu_DataService.Interfaces;
using Nu_Models.DatabaseModels;

namespace Nu_DataService.Services;

public class LibraryFileService : ILibraryFileService
{
    private readonly ILogger<LibraryFileService> _logger;
    private readonly IUnitOfWork _unitOfWork;
    
    public LibraryFileService(ILogger<LibraryFileService> logger, IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    
    public IEnumerable<LibraryFileIndex> GetAllLibraryFiles()
    {
        var files = _unitOfWork.LibraryFileIndexRepository.GetAll();
        return files;
    }
    
    public LibraryFileIndex? GetLibraryFileById(int id)
    {
        var file = _unitOfWork.LibraryFileIndexRepository.Get(id);
        return file;
    }

    
}