using Microsoft.Extensions.Logging;
using Nu_DataService.Interfaces;
using Nu_Models.DatabaseModels;
using Nu_Models.DTOs;

namespace Nu_DataService.Services;

public class LibraryFolderService : ILibraryFolderService
{
    private readonly ILogger<LibraryFolderService> _logger;
    private readonly IUnitOfWork _unitOfWork;
    
    public LibraryFolderService(ILogger<LibraryFolderService> logger, IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    
    public LibraryFolderIndex? GetLibraryRootContents()
    {
            var folder = _unitOfWork.LibraryFolderIndexRepository.GetLibraryRootFolder();
            return folder;
    }
    
    public LibraryFolderIndex? GetLibraryFolderWithChildren(int id)
    {
            var folder = _unitOfWork.LibraryFolderIndexRepository.GetLibraryFolderWithChildren(id);
            return folder;
    }
    
    public IEnumerable<LibraryFolderIndex> GetAllLibraryFolders()
    {
            var folder = _unitOfWork.LibraryFolderIndexRepository.GetAll();
            return folder;
    }
    
    public IEnumerable<LibraryFolderPathChunk> GetLibraryFolderPathChunkIds(List<LibraryFolderPathChunk> folderPathChunks)
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