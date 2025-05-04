using Microsoft.Extensions.Logging;
using Nu_Models.DatabaseModels;
using Nu_Models.Extensions.Interfaces;

namespace Nu_Models.Extensions;

public class FilePathExtensions : IFilePathExtensions
{
    private readonly ApplicationConfigurationSettings _applicationConfigurationSettings;
    private readonly ILogger<FilePathExtensions> _logger;

    public FilePathExtensions(ApplicationConfigurationSettings applicationConfigurationSettings, ILogger<FilePathExtensions> logger)
    {
        _applicationConfigurationSettings = applicationConfigurationSettings;
        _logger = logger;
    }

    public string MapImageUrlToServerUrl(LibraryFileIndex libraryFile)
    {
        var rootServerImagePath = _applicationConfigurationSettings.RootFolderPath.Replace("\\", "/");
        var filePath = libraryFile.FullName.Replace(rootServerImagePath, "");
        var finalPath = _applicationConfigurationSettings.LastFolderName + filePath;
        finalPath = finalPath.Replace("\\", "/");
        return finalPath;
    }
    
    public string MapImageUrlToServerUrl(string libraryFile)
    {
        var rootServerImagePath = _applicationConfigurationSettings.RootFolderPath.Replace("\\", "/");
        var filePath = libraryFile.Replace(rootServerImagePath, "");
        var finalPath = _applicationConfigurationSettings.LastFolderName + filePath;
        finalPath = finalPath.Replace("\\", "/");
        return finalPath;
    }

    public List<string> MapImageUrlsToServerUrls(List<LibraryFileIndex> libraryFiles)
    {
        var rootServerImagePath = _applicationConfigurationSettings.RootFolderPath.Replace("\\", "/");
        var finalPaths = new List<string>();
        foreach (var libraryFile in libraryFiles)
        {
            var filePath = libraryFile.FullName.Replace(rootServerImagePath, "");
            var finalPath = _applicationConfigurationSettings.LastFolderName + filePath;
            finalPath = finalPath.Replace("\\", "/");
            finalPaths.Add(finalPath);
        }
        return finalPaths;
    }
    
    public List<string> MapImageUrlsToServerUrls(List<string> libraryFiles)
    {
        var rootServerImagePath = _applicationConfigurationSettings.RootFolderPath.Replace("\\", "/");
        var finalPaths = new List<string>();
        foreach (var libraryFile in libraryFiles)
        {
            var filePath = libraryFile.Replace(rootServerImagePath, "");
            var finalPath = _applicationConfigurationSettings.LastFolderName + filePath;
            finalPath = finalPath.Replace("\\", "/");
            finalPaths.Add(finalPath);
        }
        return finalPaths;
    }
    
    public string GeneratePreviewThumbnailSaveLocation(string rootFilePath)
    {
        var thumbnailFolderPath = Path.Combine(rootFilePath, "Nu-PreviewThumbnails");
        if (Directory.Exists(rootFilePath))
        {
            if (!Directory.Exists(thumbnailFolderPath))
            {
                Directory.CreateDirectory(thumbnailFolderPath);
            }
        }
        return thumbnailFolderPath;
    }

    public string? GetLastFolderName(string folderPath)
    {
        folderPath = folderPath.Replace("\\", "/");
        int lastSlashIndex = folderPath.LastIndexOf('/');
        if (lastSlashIndex < 0)
        {
            // means that the folderPath is the root folder of the system directory
            return null;
        }
        else
        {
            return folderPath.Substring(lastSlashIndex + 1);
        }
    }
    
    public string NormaliseFolderPathToOs(string folderPath)
    {
        if (string.IsNullOrEmpty(folderPath))
        {
            return string.Empty;
        }

        if (!System.OperatingSystem.IsWindows())
        {
            // Replace backslashes with forward slashes
            folderPath = folderPath.Replace("\\", "/");
            // Remove any trailing slashes
            folderPath = folderPath.TrimEnd('/', '\\');
        }
        else
        {
            // Replace forward slashes with backslashes
            folderPath = folderPath.Replace("/", "\\");
            // Remove any trailing slashes
            folderPath = folderPath.TrimEnd('/', '\\');
        }
        
        return folderPath;
    }
}