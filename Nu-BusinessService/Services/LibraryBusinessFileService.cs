using Microsoft.Extensions.Logging;
using Nu_BusinessService.Interfaces;
using Nu_Models;

namespace Nu_BusinessService.Services;

public class LibraryBusinessFileService : ILibraryBusinessFileService
{
    private readonly ILogger<LibraryBusinessFileService> _logger;
    private readonly ApplicationConfigurationSettings _applicationConfigurationSettings;
    
    public LibraryBusinessFileService(ILogger<LibraryBusinessFileService> logger, ApplicationConfigurationSettings applicationConfigurationSettings)
    {
        _applicationConfigurationSettings = applicationConfigurationSettings;
        _logger = logger;
    }
    
    //TODO Look into streaming static files from disk instead of converting to base64
    public List<LibraryImageDTO> GetImageCardImageData(List<LibraryImageDTO> libraryFiles)
    {
        foreach (var libraryFile in libraryFiles)
        {
            
            var fullFilePath = Path.Combine(_applicationConfigurationSettings.FolderPathWithoutTopLevelFolder, libraryFile.FilePath, libraryFile.FileName);
            var fileData = File.ReadAllBytes(fullFilePath);
            var base64String = Convert.ToBase64String(fileData);
            libraryFile.ImageData = base64String;
        }
        return libraryFiles;
    }

}