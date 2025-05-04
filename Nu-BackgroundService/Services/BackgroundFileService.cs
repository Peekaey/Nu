using System.Collections.Concurrent;
using System.Diagnostics;
using Nu_Cache.Interfaces;
using Nu_Models;
using Nu_Models.DTOs;
using Nu_Models.Results;

namespace Nu_Cache.Services;

public class BackgroundFileService : IBackgroundFileService
{
    private readonly ILogger<BackgroundFileService> _logger;
    private readonly ApplicationConfigurationSettings _applicationConfigurationSettings;

    public BackgroundFileService(ILogger<BackgroundFileService> logger,
        ApplicationConfigurationSettings applicationConfigurationSettings)
    {
        _applicationConfigurationSettings = applicationConfigurationSettings;
        _logger = logger;
    }

    public async Task<FileReaderServiceResult> GetParentStorageFiles(string rootFolderPath, string? previewThumbnailSaveLocation = null)
    {
        rootFolderPath = rootFolderPath.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        var replacedRootFolderPath = rootFolderPath.Replace("\\", "/");
        int lastSlashIndex = replacedRootFolderPath.LastIndexOf('/');
        var folderName = string.Empty;
        var rootMasterFolderPath = string.Empty;
        var rootMasterFolderName = string.Empty;
        if (lastSlashIndex < 0)
        {
            folderName = replacedRootFolderPath;
        }
        else
        {
            var rootRootFolderPathDirectory = Directory.GetParent(replacedRootFolderPath);
            // Gets the folder path without the last folder name
            if (rootRootFolderPathDirectory != null)
            {
                var rootFolderPathDirectory = new DirectoryInfo(replacedRootFolderPath);
                rootMasterFolderPath = rootFolderPathDirectory.FullName.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                var tmpDirectory = new DirectoryInfo(rootFolderPath);
                rootMasterFolderName = tmpDirectory.Name.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            }
            else
            {
                // In the case of a Network share format like \\server\folder\ as the rootFolderPath
                rootMasterFolderName = replacedRootFolderPath.Substring(lastSlashIndex + 1);
                var trimmedPath = replacedRootFolderPath.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                rootMasterFolderPath = trimmedPath;
            }
        }
        
        // Normalise rootMasterFolderPath To Local System
        rootMasterFolderPath = rootMasterFolderPath.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
        
        // TODO Optimise this 
        return await Task.Run(() =>
        {
            var acceptedFileTypes = _applicationConfigurationSettings.AcceptedFileTypes
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            var fileDtos = Directory.EnumerateFiles(rootMasterFolderPath, "*.*", SearchOption.AllDirectories)
                .AsParallel()
                .Where(file => acceptedFileTypes.Contains(Path.GetExtension(file)) && !file.Contains(previewThumbnailSaveLocation ?? "Nu-PreviewThumbnails", StringComparison.OrdinalIgnoreCase))
                .Select(file =>
                {
                    var fileInfo = new FileInfo(file);

                    var finalFolderPath = "";
                    var finalFolderName = "";
                    var rootFolderPathDirectory = Directory.GetParent(fileInfo.DirectoryName);
                    // Gets the folder path without the last folder name
                    if (rootFolderPathDirectory != null)
                    {
                        finalFolderPath = rootFolderPathDirectory.FullName.Replace("\\", "/").TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                        var tmpDirectory = new DirectoryInfo(fileInfo.DirectoryName);
                        finalFolderName = tmpDirectory.Name.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar).Replace("\\", "/");
                    }
                    else
                    {
                        finalFolderName = fileInfo.Name.Substring(lastSlashIndex + 1).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                        var trimmedPath = fileInfo.DirectoryName.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                        var tokens = trimmedPath.TrimStart('/').Split('/');
                        finalFolderPath = $"//{tokens[0]}";
                    }

                    return new FileDTO
                    {
                        CreationTime = fileInfo.CreationTime,
                        DirectoryName = finalFolderPath + "/" + finalFolderName,
                        Extension = fileInfo.Extension,
                        FullName = fileInfo.FullName.Replace("\\", "/").TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar),
                        FileName = fileInfo.Name.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar).Replace("\\", "/"),
                        FileSize = fileInfo.Length
                    };
                })
                .ToList();
            
            Console.WriteLine($"Processed {fileDtos.Count} files in {rootFolderPath}");
            return FileReaderServiceResult.AsSuccess(fileDtos);
        });
    }
    
}