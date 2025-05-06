using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nu_BusinessService.Interfaces;
using Nu_DataService.Interfaces;
using Nu_Models;
using Nu_Models.DTOs;
using Nu_Models.Extensions.Interfaces;
using Nu_Models.Results.BusinessServiceResults;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Nu_BusinessService.Services;

public class LibraryBusinessFileService : ILibraryBusinessFileService
{
    private readonly ILogger<LibraryBusinessFileService> _logger;
    private readonly ApplicationConfigurationSettings _applicationConfigurationSettings;
    private readonly IFilePathExtensions _filePathExtensions;
    private readonly IMappingHelpers _mappingHelpers;
    private readonly ILibraryFileService _libraryFileService;
    private readonly ILibraryFolderService _libraryFolderService;
    
    public LibraryBusinessFileService(ILogger<LibraryBusinessFileService> logger, ApplicationConfigurationSettings applicationConfigurationSettings,
        IFilePathExtensions filePathExtensions, IMappingHelpers mappingHelpers, ILibraryFileService libraryFileService, ILibraryFolderService libraryFolderService)
    {
        _applicationConfigurationSettings = applicationConfigurationSettings;
        _logger = logger;
        _filePathExtensions = filePathExtensions;
        _mappingHelpers = mappingHelpers;
        _libraryFileService = libraryFileService;
        _libraryFolderService = libraryFolderService;
    }
    
    public List<LibraryImageDTO> GetImageCardImageData(List<LibraryImageDTO> libraryFiles)
    {
        foreach (var libraryFile in libraryFiles)
        {
            var fullFilePath = Path.Combine(_applicationConfigurationSettings.FolderPathWithoutTopLevelFolder, libraryFile.FilePath, libraryFile.FileName).Replace("\\", "/");
            if (libraryFile.PreviewThumbNailId != null)
            {
                // Return path of the preview thumbnail
                var cacheFolderName = _filePathExtensions.GetLastFolderName(_filePathExtensions.GeneratePreviewThumbnailSaveLocation(_applicationConfigurationSettings.RootFolderPath));
                var imageServicePath = _applicationConfigurationSettings.LastFolderName + "/" + cacheFolderName + "/" + _filePathExtensions.GetLastFolderName(libraryFile.FilePath) + "-" + libraryFile.FileName + ".jpg";
                libraryFile.ServerImagePath = imageServicePath;
            }
            else
            {
                // Return path of the original image
                libraryFile.ServerImagePath = _filePathExtensions.MapImageUrlToServerUrl(fullFilePath);
            }
            
        }
        return libraryFiles;
    }
    
    public BusinessServiceResult<List<LibraryImageDTO>> GetLibraryFolderImageContent(int id)
    {
        try {
            var album = _libraryFolderService.GetLibraryFolderWithChildren(id);
        
            if (album == null)
            {
                return BusinessServiceResult<List<LibraryImageDTO>>.NotFound();
            }
        
            List<LibraryImageDTO> imageCardDtOs = new List<LibraryImageDTO>();
        
            foreach (var image in album.LibraryFiles)
            {
                var imageCardDto = new LibraryImageDTO
                {
                    Id = image.Id,
                    FileName= image.FileName,
                    FilePath =  image.DirectoryName,
                    FileSizeMb = image.FileSizeInMB,
                    FileType = image.FileType,
                    SystemCreationTime = image.SystemCreationTime,
                    PreviewThumbNailId = image.LibraryPreviewThumbnailIndexId ?? null

                };
                imageCardDtOs.Add(imageCardDto);
            }
            imageCardDtOs = GetImageCardImageData(imageCardDtOs);
        
            return BusinessServiceResult<List<LibraryImageDTO>>.Ok(imageCardDtOs);
        } 
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving album content for ID {Id}", id);
            return BusinessServiceResult<List<LibraryImageDTO>>.Error("An error occurred while retrieving the album content.");
        }
    }
    
    public BusinessServiceResult<string?> GetImagePathById(int id)
    {
        try {
            var file = _libraryFileService.GetLibraryFileById(id);
            if (file == null)
            {
                return BusinessServiceResult<string?>.NotFound();
            }
            var fullFilePath = Path.Combine(_applicationConfigurationSettings.FolderPathWithoutTopLevelFolder, file.DirectoryName, file.FileName);
            var imageData = _filePathExtensions.MapImageUrlToServerUrl(fullFilePath);
            return BusinessServiceResult<string?>.Ok(imageData);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving image path for file with ID {Id}", id);
            return BusinessServiceResult<string?>.Error("An error occurred while retrieving the image path.");
        }
    }
    
    public BusinessServiceResult<FileStreamResult?> DownloadFile(int id)
    {
        try
        {
            var file = _libraryFileService.GetLibraryFileById(id);
            if (file == null)
            {
                return BusinessServiceResult<FileStreamResult?>.NotFound();
            }

            var fullFilePath = Path.Combine(_applicationConfigurationSettings.FolderPathWithoutTopLevelFolder,
                file.DirectoryName, file.FileName);

            if (!System.IO.File.Exists(fullFilePath))
            {
                return BusinessServiceResult<FileStreamResult?>.Error("Original File not found");
            }

            var stream = new FileStream(fullFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            var mimeType = GetMimeType(Path.GetExtension(file.FileName));

            return BusinessServiceResult<FileStreamResult?>.Ok(new FileStreamResult(stream, mimeType)
            {
                FileDownloadName = file.FileName
            });
        } 
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error downloading file with ID {Id}", id);
            return BusinessServiceResult<FileStreamResult?>.Error("An error occurred while downloading the file.");
        }
    }
    
    private string GetMimeType(string fileExtension)
    {
        if (string.IsNullOrEmpty(fileExtension))
            return "application/octet-stream";

        return fileExtension.ToLower() switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".gif" => "image/gif",
            ".bmp" => "image/bmp",
            ".webp" => "image/webp",
            ".tiff" or ".tif" => "image/tiff",
            ".pdf" => "application/pdf",
            ".txt" => "text/plain",
            ".mp4" => "video/mp4",
            ".mov" => "video/quicktime",
            ".avi" => "video/x-msvideo",
            ".wmv" => "video/x-ms-wmv",
            ".mp3" => "audio/mpeg",
            ".wav" => "audio/wav",
            _ => "application/octet-stream"
        };
    }
    
    
}