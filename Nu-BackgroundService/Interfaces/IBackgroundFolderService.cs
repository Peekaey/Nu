using Nu_Models.DTOs;
using Nu_Models.Results;

namespace Nu_Cache.Interfaces;

public interface IBackgroundFolderService
{
    FolderReaderServiceResult GetStorageFolders(string rootFolderPath, string? previewThumbnailSaveLocation = null);
    bool FolderExists(string rootFolderPath);
}