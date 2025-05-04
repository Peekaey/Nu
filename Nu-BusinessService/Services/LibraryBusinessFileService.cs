using Microsoft.Extensions.Logging;
using Nu_BusinessService.Interfaces;
using Nu_Models;
using Nu_Models.Extensions.Interfaces;
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
    
    
    public LibraryBusinessFileService(ILogger<LibraryBusinessFileService> logger, ApplicationConfigurationSettings applicationConfigurationSettings, IFilePathExtensions filePathExtensions)
    {
        _applicationConfigurationSettings = applicationConfigurationSettings;
        _logger = logger;
        _filePathExtensions = filePathExtensions;
    }
    

    public List<LibraryImageDTO> GetImageCardImageData(List<LibraryImageDTO> libraryFiles)
    {
        var basePreviewThumbnailLocation = _filePathExtensions.GeneratePreviewThumbnailSaveLocation(_applicationConfigurationSettings.RootFolderPath);
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
}