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
    public IList<LibraryPreviewThumbnailIndex> MapPreviewThumbnailDtoToLibraryPreviewThumbnailIndex(List<PreviewThumbnailDTO> previewThumbnailDtos, List<LibraryFileIndex> fileIndexes)
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

    public IList<LibraryFolderPathChunk> MapFolderPathToChunks(string folderPath)
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
}