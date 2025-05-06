using Nu_Models.DatabaseModels;
using Nu_Models.DTOs;

namespace Nu_Models.Extensions.Interfaces;

public interface IMappingHelpers
{
    IEnumerable<LibraryPreviewThumbnailIndex> MapPreviewThumbnailDtoToLibraryPreviewThumbnailIndex(
        IEnumerable<PreviewThumbnailDTO> previewThumbnailDtos, IEnumerable<LibraryFileIndex> fileIndexes);
    IEnumerable<LibraryFolderPathChunk> MapFolderPathToChunks(string folderPath);
    IEnumerable<LibraryFolderIndex> MapFolderDtoToLibraryFolderIndex(List<FolderDTO> folders);
    IEnumerable<LibraryFileIndex> MapFileDtoToLibraryFileIndex(List<FileDTO> files);
}