using Nu_Cache.Interfaces;
using Nu_Models.DTOs;
using Nu_Models.Enums;
using Nu_Models.Results;

namespace Nu_Cache.Services;

public class BackgroundFolderService : IBackgroundFolderService
{
    private readonly ILogger<BackgroundFolderService> _logger;
    
    public BackgroundFolderService(ILogger<BackgroundFolderService> logger)
    {
        _logger = logger;
    }

    public FolderReaderServiceResult GetStorageFolders(string rootFolderPath)
    {
        try
        {
            rootFolderPath = rootFolderPath.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            var folders = new List<FolderDTO>();
            var directories = Directory.GetDirectories(rootFolderPath, "*", SearchOption.AllDirectories);

            // Add Root Folder
            var rootFolder = new FolderDTO
            {
                FolderName = Path.GetFileName(rootFolderPath),
                FolderPath = string.Empty,
                FolderLevel = FolderLevel.Root,
                ParentFolder = null
            };
            folders.Add(rootFolder);
            
            // Add Child Folders After Root Folder
            foreach (var directory in directories)
            {
                var folderPathAfterRoot = directory.Substring(rootFolderPath.Length).TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar).Replace("\\", "/");
                var directoryFolderLevel = folderPathAfterRoot.Split('/').Length;
                var parentFolderPath = Path.GetDirectoryName(directory)?.Substring(rootFolderPath.Length).TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar).Replace("\\", "/");
                var parentFolder = folders.FirstOrDefault(f => f.FolderPath == parentFolderPath);

                var lastFolderName = Path.GetFileName(rootFolderPath);
                var folderPath2 = Path.Combine(lastFolderName,
                    directory.Substring(rootFolderPath.Length).TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)).Replace("\\", "/");
                
                var folder = new FolderDTO
                {
                    FolderName = Path.GetFileName(directory),
                    FolderPath = folderPath2,
                    FolderLevel = (FolderLevel)directoryFolderLevel,
                    ParentFolder = parentFolder ?? null
                };

                folders.Add(folder);
            }

            return FolderReaderServiceResult.AsSuccess(folders);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occured while reading folders from directory");
            return FolderReaderServiceResult.AsFailure("An error occured while reading folders from directory");
        }
    }

    public bool FolderExists(string rootFolderPath)
    {
        if (!Directory.Exists(rootFolderPath))
        {
            _logger.LogError("Parent storage folder does not exist");
            return false;
        }
        return true;
    }
}