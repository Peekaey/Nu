using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Nu_Models.DatabaseModels;
using Nu_Models.DTOs;
using Nu_Models.Extensions.Interfaces;

namespace Nu_Models.Extensions;

// Budget Automapper
public class MappingHelpers : IMappingHelpers
{
    private readonly ILogger<MappingHelpers> _logger;
    private readonly ApplicationConfigurationSettings _applicationConfigurationSettings;
    
    public MappingHelpers(ILogger<MappingHelpers> logger, ApplicationConfigurationSettings applicationConfigurationSettings)
    {
        _logger = logger;
        _applicationConfigurationSettings = applicationConfigurationSettings;
    }
    public IEnumerable<LibraryPreviewThumbnailIndex> MapPreviewThumbnailDtoToLibraryPreviewThumbnailIndex(IEnumerable<PreviewThumbnailDTO> previewThumbnailDtos, IEnumerable<LibraryFileIndex> fileIndexes)
    {
        var previewThumbnailIndexes = new List<LibraryPreviewThumbnailIndex>();

        foreach (var previewThumbnailDto in previewThumbnailDtos)
        {
            LibraryPreviewThumbnailIndex libraryThumbnailIndex = new LibraryPreviewThumbnailIndex();
            var topLevelFolderNameIndex = previewThumbnailDto.FileName.IndexOf("-");
            var topLevelFolderName = previewThumbnailDto.FileName.Substring(0, topLevelFolderNameIndex);
            
            
            var remainingFileNameContent = previewThumbnailDto.FileName.Substring(topLevelFolderNameIndex + 1);
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(remainingFileNameContent);
            
            var fileIndex = fileIndexes.FirstOrDefault(fd => Path.GetFileName(fd.DirectoryName) == topLevelFolderName && fd.FileName == fileNameWithoutExtension);
            if (fileIndex != null)
            {
                libraryThumbnailIndex.FileName = fileNameWithoutExtension;
                libraryThumbnailIndex.LibraryFileIndexId = fileIndex.Id;
                libraryThumbnailIndex.DirectoryName = topLevelFolderName;
                libraryThumbnailIndex.FileType = fileIndex.FileType;
                libraryThumbnailIndex.FullName = previewThumbnailDto.FullName;
                libraryThumbnailIndex.SystemCreationTime = previewThumbnailDto.CreationTime;
                libraryThumbnailIndex.CreatedDate = DateTime.UtcNow;
                libraryThumbnailIndex.LastUpdatedDate = DateTime.UtcNow;
                previewThumbnailIndexes.Add(libraryThumbnailIndex);
            }
            else
            {
                Debug.WriteLine("File index not found for: " + previewThumbnailDto.FullName +"Ignoring.... ");
            }

        }
        var nullDirectoryNameIndexes = previewThumbnailIndexes.Where(x => x.DirectoryName == null).ToList();
        return previewThumbnailIndexes;
    }

    public IEnumerable<LibraryFolderPathChunk> MapFolderPathToChunks(string folderPath)
    {
        folderPath = folderPath.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar).Replace("\\", "/");
        folderPath = folderPath.Replace(_applicationConfigurationSettings.NormalisedRootFolderPath, string.Empty);
        
        List<LibraryFolderPathChunk> libraryFolderPathChunks = new List<LibraryFolderPathChunk>();
        libraryFolderPathChunks.Add(new LibraryFolderPathChunk
        {
            FolderName = _applicationConfigurationSettings.LastFolderName
        });
        
        var chunks = folderPath.Split("/")
            .Where(chunk => !string.IsNullOrWhiteSpace(chunk))
            .Select(chunk => new LibraryFolderPathChunk { FolderName = chunk })
            .ToList();

        libraryFolderPathChunks.AddRange(chunks);
        return libraryFolderPathChunks;
    }

    public IEnumerable<LibraryFolderIndex> MapFolderDtoToLibraryFolderIndex(List<FolderDTO> folders)
    {
        return folders.Select(f => new LibraryFolderIndex
        {
            FolderName = f.FolderName,
            FolderPath = f.FolderPath,
            FolderLevel = (int)f.FolderLevel,
            CreatedDate = DateTime.UtcNow,
            LastUpdatedDate = DateTime.UtcNow
        });
    }

    public IEnumerable<LibraryFileIndex> MapFileDtoToLibraryFileIndex(List<FileDTO> files)
    {
        return files.Select(f => new LibraryFileIndex
        {
            DirectoryName = f.DirectoryName,
            FileType = f.Extension,
            FullName = f.FullName,
            FileName = f.FileName,
            FileSizeInMB = f.FileSizeInMB,
            CreatedDate = DateTime.UtcNow,
            LastUpdatedDate = DateTime.UtcNow,
            SystemCreationTime = DateTimeExtensions.ConvertDateTimeToUtc(f.CreationTime)
        });
    }
    
    
}