using Nu_Cache.Interfaces;
using Nu_DataService.Interfaces;
using Nu_Models.DTOs;
using Nu_Models.Extensions;
using Nu_Models.Extensions.Interfaces;
using Nu_Models.Results;

namespace Nu_Cache.Services;

public class BackgroundOrchestratorService : IBackgroundOrchestratorService
{
    private readonly IBackgroundFileService _backgroundFileService;
    private readonly IBackgroundFolderService _backgroundFolderService;
    private readonly IBackgroundResizeService _backgroundResizeService;
    private readonly IIndexingService _indexingService;
    private readonly IFilePathExtensions _filePathExtensions;
    
    private readonly ILogger<BackgroundOrchestratorService> _logger;

    public BackgroundOrchestratorService(IBackgroundFileService backgroundFileService, IBackgroundFolderService backgroundFolderService,
        IIndexingService indexingService, IBackgroundResizeService backgroundResizeService , IFilePathExtensions filePathExtensions,
        ILogger<BackgroundOrchestratorService> logger)
    {
        _backgroundFileService = backgroundFileService;
        _backgroundFolderService = backgroundFolderService;
        _backgroundResizeService = backgroundResizeService;
        _logger = logger;
        _indexingService = indexingService;
        _filePathExtensions = filePathExtensions;
    }

    public async Task<ServiceResult> IndexLibraryContents(string rootFolderPath)
    {
        _logger.LogInformation("Start of IndexLibraryContents in BackgroundOrchestratorService");
        var folderExists = _backgroundFolderService.FolderExists(rootFolderPath);

        if (folderExists == false)
        {
            return ServiceResult.AsFailure("Root Folder Does Not Exist");
        } 
        
        var previewThumbnailSaveLocation = _filePathExtensions.GeneratePreviewThumbnailSaveLocation(rootFolderPath);
        
        var foldersResult = _backgroundFolderService.GetStorageFolders(rootFolderPath, previewThumbnailSaveLocation);

        if (foldersResult.Success == false)
        {
            return ServiceResult.AsFailure(foldersResult.Error);
        }
        _logger.LogInformation("Folders Found: {FoldersCount}", foldersResult.Folders.Count);

        var filesResult = await _backgroundFileService.GetParentStorageFiles(rootFolderPath, previewThumbnailSaveLocation);
        if (filesResult.Success == false)
        {
            return ServiceResult.AsFailure(filesResult.Error);
        }
        
        _logger.LogInformation("Files Found: {FilesCount}", filesResult.Files.Count);
        
        var updateIndexResult = _indexingService.IndexLibraryContents(foldersResult.Folders, filesResult.Files);
        
        if (updateIndexResult.Success == false)
        {
            return ServiceResult.AsFailure(updateIndexResult.Error);
        }
        
        _logger.LogInformation("Folder and Files Indexed Successfully");
        
        var resizeResult = _backgroundResizeService.GeneratePreviewThumbnails(filesResult.Files, rootFolderPath);
        
        if (resizeResult.Success == false)
        {
            return ServiceResult.AsFailure(resizeResult.Error);
        }
        
        _logger.LogInformation("Preview Thumbnails Generated Successfully");
        
        var indexPreviewThumbnailResult = _indexingService.IndexPreviewThumbnails(resizeResult.Files);
        
        if (indexPreviewThumbnailResult.Success == false)
        {
            return ServiceResult.AsFailure(indexPreviewThumbnailResult.Error);
        }
        
        _logger.LogInformation("Preview Thumbnails Indexed Successfully");
        
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