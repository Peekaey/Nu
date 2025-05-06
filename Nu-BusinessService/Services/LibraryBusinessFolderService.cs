using Microsoft.Extensions.Logging;
using Nu_BusinessService.Interfaces;
using Nu_DataService.Interfaces;
using Nu_Models;
using Nu_Models.DatabaseModels;
using Nu_Models.DTOs;
using Nu_Models.Extensions;
using Nu_Models.Extensions.Interfaces;
using Nu_Models.Results.BusinessServiceResults;

namespace Nu_BusinessService.Services;

public class LibraryBusinessFolderService : ILibraryBusinessFolderService
{
    private readonly ILogger<LibraryBusinessFolderService> _logger;
    private readonly ApplicationConfigurationSettings _applicationConfigurationSettings;
    private readonly ILibraryBusinessFileService _libraryBusinessFileService;
    private readonly FilePathExtensions _filePathExtensions;
    private readonly IMappingHelpers _mappingHelpers;
    private readonly ILibraryFolderService _libraryFolderService;
    
    public LibraryBusinessFolderService(ILogger<LibraryBusinessFolderService> logger, ILibraryFolderService libraryFolderService, 
        ApplicationConfigurationSettings applicationConfigurationSettings, ILibraryBusinessFileService libraryBusinessFileService, 
        FilePathExtensions filePathExtensions, IMappingHelpers mappingHelpers)
    {
        _applicationConfigurationSettings = applicationConfigurationSettings;
        _logger = logger;
        _libraryFolderService = libraryFolderService;
        _libraryBusinessFileService = libraryBusinessFileService;
        _filePathExtensions = filePathExtensions;
        _mappingHelpers = mappingHelpers;
    }
    
    public BusinessServiceResult<LibraryFolderContentsDTO?> GetLibraryRootContents()
    {
        try {
            var folder = _libraryFolderService.GetLibraryRootContents();
        
            if (folder == null)
            {
                return BusinessServiceResult<LibraryFolderContentsDTO?>.Error("No root folder found");
            }
        
            LibraryFolderContentsDTO libraryFolderContentsDto = new LibraryFolderContentsDTO
            {
                Files = [],
                Folders = []
            };
        
            foreach (var image in folder.LibraryFiles)
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
                libraryFolderContentsDto.Files.Add(imageCardDto);
            }
            libraryFolderContentsDto.Files = _libraryBusinessFileService.GetImageCardImageData(libraryFolderContentsDto.Files);
        
            foreach (var album in folder.ChildFolders)
            {
                LibraryAlbumDTO albumDto = new LibraryAlbumDTO
                {
                    Id = album.Id,
                    AlbumName = album.FolderName
                };
                libraryFolderContentsDto.Folders.Add(albumDto);
            }
            return BusinessServiceResult<LibraryFolderContentsDTO?>.Ok(libraryFolderContentsDto);
        } 
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving root folder contents");
            return BusinessServiceResult<LibraryFolderContentsDTO?>.Error("Error retrieving root folder contents");
        }
    }
    
    public BusinessServiceResult<LibraryFolderContentsDTO?> GetLibraryFolderContents(int id)
    {
        try {
            var folder = _libraryFolderService.GetLibraryFolderWithChildren(id);
        
            if (folder == null)
            {
                return BusinessServiceResult<LibraryFolderContentsDTO?>.NotFound();
            }
        
            LibraryFolderContentsDTO libraryFolderContentsDto = new LibraryFolderContentsDTO();
            libraryFolderContentsDto.Files = new List<LibraryImageDTO>();
            libraryFolderContentsDto.Folders = new List<LibraryAlbumDTO>();
            libraryFolderContentsDto.FolderPath = _mappingHelpers.MapFolderPathToChunks(folder.FolderPath).ToList();
            libraryFolderContentsDto.FolderPath = _libraryFolderService.GetLibraryFolderPathChunkIds(libraryFolderContentsDto.FolderPath).ToList();
            foreach (var image in folder.LibraryFiles)
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
                libraryFolderContentsDto.Files.Add(imageCardDto);
            }
            libraryFolderContentsDto.Files = _libraryBusinessFileService.GetImageCardImageData(libraryFolderContentsDto.Files);
        
            foreach (var album in folder.ChildFolders)
            {
                LibraryAlbumDTO albumDto = new LibraryAlbumDTO
                {
                    Id = album.Id,
                    AlbumName = album.FolderName
                };
            
                albumDto.AlbumPath = album.FolderPath;
            
                libraryFolderContentsDto.Folders.Add(albumDto);
            }

            return BusinessServiceResult<LibraryFolderContentsDTO?>.Ok(libraryFolderContentsDto);
        } 
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving folder contents for folder with ID {Id}", id);
            return BusinessServiceResult<LibraryFolderContentsDTO?>.Error("Error retrieving folder contents");
        }
    }
    
    public BusinessServiceResult<List<LibraryAlbumDTO>> GetAllFolders()
    {
        try
        {
            var albums = _libraryFolderService.GetAllLibraryFolders().ToList();

            if (albums.Count == 0)
            {
                return BusinessServiceResult<List<LibraryAlbumDTO>>.NotFound();
            }

            List<LibraryAlbumDTO> albumDtOs = new List<LibraryAlbumDTO>();

            foreach (var album in albums)
            {
                LibraryAlbumDTO albumDto = new LibraryAlbumDTO
                {
                    Id = album.Id,
                    AlbumName = album.FolderName
                };

                if (album.FolderLevel == 0)
                {
                    albumDto.AlbumPath = "Root Library Folder";
                }
                else
                {
                    albumDto.AlbumPath = _applicationConfigurationSettings.TopLevelFolderName + "/" + album.FolderPath;
                }

                albumDtOs.Add(albumDto);
            }

            return BusinessServiceResult<List<LibraryAlbumDTO>>.Ok(albumDtOs);
        } 
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all folders");
            return BusinessServiceResult<List<LibraryAlbumDTO>>.Error("Error retrieving all folders");
        }
    }
    
}