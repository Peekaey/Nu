using Nu_Cache.Interfaces;
using Nu_DataService.Interfaces;
using Nu_Models.DTOs;
using Nu_Models.Results;

namespace Nu_Cache.Services;

public class BackgroundOrchestratorService : IBackgroundOrchestratorService
{
    private readonly IBackgroundFileService _backgroundFileService;
    private readonly IBackgroundFolderService _backgroundFolderService;
    private readonly IIndexingService _indexingService;
    private readonly ILogger<BackgroundOrchestratorService> _logger;
    private readonly string filePath = "";

    public BackgroundOrchestratorService(IBackgroundFileService backgroundFileService, IBackgroundFolderService backgroundFolderService,
        IIndexingService indexingService, ILogger<BackgroundOrchestratorService> logger)
    {
        _backgroundFileService = backgroundFileService;
        _backgroundFolderService = backgroundFolderService;
        _logger = logger;
        _indexingService = indexingService;
    }

    public async Task<ServiceResult> IndexLibraryContents(string rootFolderPath)
    {
        rootFolderPath = filePath;
        var folderExists = _backgroundFolderService.FolderExists(rootFolderPath);

        if (folderExists == false)
        {
            return ServiceResult.AsFailure("Root Folder Does Not Exist");
        }
        
        var filesResult = await _backgroundFileService.GetParentStorageFiles(filePath);
        if (filesResult.Success == false)
        {
            return ServiceResult.AsFailure(filesResult.Error);
        }

        var foldersResult = _backgroundFolderService.GetStorageFolders(filePath);

        if (foldersResult.Success == false)
        {
            return ServiceResult.AsFailure(foldersResult.Error);
        }

        var updateIndexResult = _indexingService.IndexLibraryContents(foldersResult.Folders, filesResult.Files);
        
        if (updateIndexResult.Success == false)
        {
            return ServiceResult.AsFailure(updateIndexResult.Error);
        }
        
        return ServiceResult.AsSuccess();
        
        
    }

    public async Task<List<FileDTO>> GetLibraryContents()
    {
        var filesResult = await _backgroundFileService.GetParentStorageFiles("");
        if (filesResult.Success == false)
        {
            return new List<FileDTO>();
        }
        return filesResult.Files;
        
    }
}