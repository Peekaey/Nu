using System.Diagnostics;
using Nu_Cache.Interfaces;
using Nu_Models.DTOs;
using Nu_Models.Results;

namespace Nu_Cache.Services;

public class BackgroundFileService : IBackgroundFileService
{
    private readonly ILogger<BackgroundFileService> _logger;

    public BackgroundFileService(ILogger<BackgroundFileService> logger)
    {
        _logger = logger;
    }

    public async Task<FileReaderServiceResult> GetParentStorageFiles(string rootFolderPath)
    {
        rootFolderPath = rootFolderPath.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        
        try
        {
            var files = Directory.EnumerateFiles(rootFolderPath, "*.*", SearchOption.AllDirectories)
                .Select(file => new FileInfo(file))
                .ToList();

            List<FileDTO> fileDtos = new List<FileDTO>();

            foreach (var file in files)
            {
                var parentDirectory = Path.GetDirectoryName(rootFolderPath);
                var directoryName = file.DirectoryName.Substring(parentDirectory.Length)
                    .TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar).Replace("\\", "/");
                
                var fullNameIncludingRootPath = file.FullName.Substring(parentDirectory.Length)
                    .TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar).Replace("\\", "/");
                
                FileDTO fileDto = new FileDTO();
                fileDto.CreationTime = file.CreationTime;
                fileDto.DirectoryName = directoryName;
                fileDto.Extension = file.Extension;
                fileDto.FullName = fullNameIncludingRootPath;
                fileDto.FileName = file.Name;
                fileDto.FileSize = file.Length;
                fileDtos.Add(fileDto);
            }
            
            
            return FileReaderServiceResult.AsSuccess(fileDtos);

        } 
        catch (Exception e)
        {
            _logger.LogError(e, "An error occurred while reading files from the directory");
            return FileReaderServiceResult.AsFailure("An error occurred while reading files from the directory");
        }
    }
    
}