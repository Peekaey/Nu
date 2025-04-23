using Microsoft.Extensions.Logging;
using Nu_DataService.Interfaces;
using Nu_Models.DatabaseModels;

namespace Nu_DataService.Services;

public class LibraryService : ILibraryService
{
    private readonly ILogger<LibraryService> _logger;
    private readonly IUnitOfWork _unitOfWork;
    
    public LibraryService(ILogger<LibraryService> logger, IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    
    public LibraryFolderIndex? GetLibraryRootContents()
    {
        try
        {
            var folder = _unitOfWork.LibraryFolderIndexRepository.GetLibraryRootFolder();
            return folder;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting library folder indexes");
            throw;
        }
    }
    
    public List<LibraryFolderIndex> GetAllLibraryFolders()
    {
        try
        {
            var folder = _unitOfWork.LibraryFolderIndexRepository.GetAll();
            return folder;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting library folder indexes");
            throw;
        }
    }

    public LibraryFolderIndex GetLibraryFolderWithChildren(int id)
    {
        try
        {
            var folder = _unitOfWork.LibraryFolderIndexRepository.GetLibraryFolderWithChildren(id);
            return folder;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting library folder indexes");
            throw;
        }
    }
    
    
}