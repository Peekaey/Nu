﻿using Nu_Models.DatabaseModels;
using Nu_Models.DTOs;
using Nu_Models.Results;

namespace Nu_DataService.Interfaces;

public interface IIndexingService
{
    ServiceResult IndexLibraryContents(List<FolderDTO> folders, List<FileDTO> files);
    ServiceResult IndexPreviewThumbnails(List<PreviewThumbnailDTO> previewThumbnailDtos);
}