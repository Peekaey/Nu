using Nu_Models.DatabaseModels;
using Nu_Models.DTOs;

namespace Nu_Models.Extensions.Interfaces;

public interface IMappingHelpers
{
    IList<LibraryPreviewThumbnailIndex> MapPreviewThumbnailDtoToLibraryPreviewThumbnailIndex(
        List<PreviewThumbnailDTO> previewThumbnailDtos, List<LibraryFileIndex> fileIndexes);
    IList<LibraryFolderPathChunk> MapFolderPathToChunks(string folderPath);
}