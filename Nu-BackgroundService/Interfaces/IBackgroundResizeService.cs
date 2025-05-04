using Nu_Models.DTOs;
using Nu_Models.Results;

namespace Nu_Cache.Interfaces;

public interface IBackgroundResizeService
{ 
    PreviewThumbnailGenerateResult GeneratePreviewThumbnails(List<FileDTO> fileDtos, string rootFolderPath);
}