using Microsoft.Extensions.Logging;
using Nu_DataService.Interfaces;
using Nu_Models.DatabaseModels;
using Nu_Models.DTOs;

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

    public IList<LibraryFileIndex> GetAllLibraryFiles()
    {
        try
        {
            var files = _unitOfWork.LibraryFileIndexRepository.GetAll();
            return files;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting library file indexes");
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

    public LibraryFileIndex GetLibraryFileById(int id)
    {
        try
        {
            var file = _unitOfWork.LibraryFileIndexRepository.Get(id);
            return file;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting library file indexes");
            throw;
        }
    }

    public List<LibraryFolderPathChunk> GetLibraryFolderPathChunkIds(List<LibraryFolderPathChunk> folderPathChunks)
    {
        var folderNames = folderPathChunks.Select(f => f.FolderName).ToList();
        var folders = _unitOfWork.LibraryFolderIndexRepository.GetFoldersByFolderName(folderNames);
        foreach (var folder in folderPathChunks)
        {
            var existingFolder = folders.FirstOrDefault(f => f.FolderName == folder.FolderName);
            if (existingFolder != null)
            {
                folder.Id = existingFolder.Id;
            }
        }
        return folderPathChunks;
    }
    
    
    
}