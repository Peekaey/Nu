using Nu_Cache.Interfaces;
using Nu_Models;
using Nu_Models.DTOs;
using Nu_Models.Enums;
using Nu_Models.Results;

namespace Nu_Cache.Services;

public class BackgroundFolderService : IBackgroundFolderService
{
    private readonly ILogger<BackgroundFolderService> _logger;
    private readonly ApplicationConfigurationSettings _applicationConfigurationSettings;
    
    public BackgroundFolderService(ILogger<BackgroundFolderService> logger, ApplicationConfigurationSettings applicationConfigurationSettings)
    {
        _logger = logger;
        _applicationConfigurationSettings = applicationConfigurationSettings;
    }

public FolderReaderServiceResult GetStorageFolders(string rootFolderPath)
{
    try
    {
        rootFolderPath = rootFolderPath.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        var folders = new List<FolderDTO>();
        var replacedRootFolderPath = rootFolderPath.Replace("\\", "/");
        int lastSlashIndex = replacedRootFolderPath.LastIndexOf('/');
        
        var rootFolder = new FolderDTO
        {
            FolderName = "",
            FolderPath = string.Empty,
            FolderLevel = FolderLevel.Root,
            ParentFolder = null
        };
        
        if (lastSlashIndex < 0)
        {
            rootFolder.FolderName = replacedRootFolderPath;
        }
        else
        {
            var rootRootFolderPathDirectory = Directory.GetParent(replacedRootFolderPath);
            // Gets the folder path without the last folder name
            if (rootRootFolderPathDirectory != null)
            {
                var rootFolderPathDirectory = new DirectoryInfo(replacedRootFolderPath);
                rootFolder.FolderPath = rootFolderPathDirectory.FullName.Replace("\\", "/").TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

                var tmpDirectory = new DirectoryInfo(rootFolderPath);
                rootFolder.FolderName = tmpDirectory.Name.Replace("\\", "/").TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            }
            else
            {
                // In the case of a Network share format like \\server\folder\ as the rootFolderPath
                rootFolder.FolderName = replacedRootFolderPath.Substring(lastSlashIndex + 1);
                var trimmedPath = replacedRootFolderPath.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                rootFolder.FolderPath = trimmedPath;
            }
        }

        folders.Add(rootFolder);
        var directories = Directory.GetDirectories(rootFolderPath, "*", SearchOption.AllDirectories);

        foreach (var directory in directories)
        {
            var lastFolderInDirectoryPath = directory.Substring(rootFolderPath.Length)
                .TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
                                       .Replace("\\", "/");

            // Determine the folder level (number of segments in the relative path).
            var folderLevel = lastFolderInDirectoryPath.Split('/').Length;
            var finalFolderPath = "";
            var finalFolderName = "";
            var rootFolderPathDirectory = Directory.GetParent(directory);
            var directoryDirectory = new DirectoryInfo(directory);
            // Gets the folder path without the last folder name
            if (rootFolderPathDirectory != null)
            {
                finalFolderPath = directoryDirectory.FullName.Replace("\\", "/").TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                var tmpDirectory = new DirectoryInfo(directory);
                finalFolderName = tmpDirectory.Name.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar).Replace("\\", "/");
            }
            else
            {
                // In the case of a Network share format like \\server\folder\ as the rootFolderPath
                finalFolderName = directory.Substring(lastSlashIndex + 1).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                var trimmedPath = directory.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                var tokens = trimmedPath.TrimStart('/').Split('/');
                finalFolderPath = $"//{tokens[0]}";                
            }
            
            // Create a new FolderDTO using the relative path.
            var folder = new FolderDTO
            {
                FolderName = finalFolderName,
                FolderPath = finalFolderPath,
                FolderLevel = (FolderLevel)folderLevel,
                ParentFolder = null
            };

            folders.Add(folder);
        }
        
        return FolderReaderServiceResult.AsSuccess(folders);
    }
    catch (Exception e)
    {
        _logger.LogError(e, "An error occurred while reading folders from directory");
        return FolderReaderServiceResult.AsFailure("An error occurred while reading folders from directory");
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