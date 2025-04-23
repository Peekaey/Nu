using Microsoft.Extensions.Logging;
using Nu_BusinessService.Interfaces;
using Nu_DataService.Interfaces;
using Nu_Models;
using Nu_Models.DatabaseModels;
using Nu_Models.DTOs;

namespace Nu_BusinessService;

// TODO Clean up spaghetti code and organise into separate services
public class LibraryBusinessService : ILibraryBusinessService
{
    private readonly ILogger<LibraryBusinessService> _logger;
    private readonly ILibraryService _libraryService;
    private readonly ApplicationConfigurationSettings _applicationConfigurationSettings;
    private readonly ILibraryBusinessFileService _libraryBusinessFileService;
    
    public LibraryBusinessService(ILogger<LibraryBusinessService> logger, ILibraryService libraryService, 
        ApplicationConfigurationSettings applicationConfigurationSettings, ILibraryBusinessFileService libraryBusinessFileService)
    {
        _applicationConfigurationSettings = applicationConfigurationSettings;
        _logger = logger;
        _libraryService = libraryService;
        _libraryBusinessFileService = libraryBusinessFileService;
    }
    
    public LibraryFolderContentsDTO GetLibraryRootContents()
    {
        var folder = _libraryService.GetLibraryRootContents();
        LibraryFolderContentsDTO libraryFolderContentsDTO = new LibraryFolderContentsDTO();
        libraryFolderContentsDTO.Files = new List<LibraryImageDTO>();
        libraryFolderContentsDTO.Folders = new List<LibraryAlbumDTO>();
        foreach (var image in folder.LibraryFiles)
        {
            var imageCardDTO = new LibraryImageDTO
            {
                Id = image.Id,
                FileName= image.FileName,
                FilePath =  image.DirectoryName,
                FileSizeMb = image.FileSizeInMB,
                FileType = image.FileType,
                SystemCreationTime = image.SystemCreationTime
            };
            libraryFolderContentsDTO.Files.Add(imageCardDTO);
        }
        libraryFolderContentsDTO.Files = _libraryBusinessFileService.GetImageCardImageData(libraryFolderContentsDTO.Files);
        
        foreach (var album in folder.ChildFolders)
        {
            LibraryAlbumDTO albumDto = new LibraryAlbumDTO();
            albumDto.Id = album.Id;
            albumDto.AlbumName = album.FolderName;
            
            if (album.FolderLevel == 0)
            {
                albumDto.AlbumPath = album.FolderPath;
            }
            else
            {
                albumDto.AlbumPath = album.FolderPath;
            }

            libraryFolderContentsDTO.Folders.Add(albumDto);
        }

        return libraryFolderContentsDTO;
    }
    
    public LibraryFolderContentsDTO GetLibraryFolderContents(int id)
    {
        var folder = _libraryService.GetLibraryFolderWithChildren(id);
        LibraryFolderContentsDTO libraryFolderContentsDTO = new LibraryFolderContentsDTO();
        libraryFolderContentsDTO.Files = new List<LibraryImageDTO>();
        libraryFolderContentsDTO.Folders = new List<LibraryAlbumDTO>();
        libraryFolderContentsDTO.FolderPath = folder.FolderPath;
        foreach (var image in folder.LibraryFiles)
        {
            var imageCardDTO = new LibraryImageDTO
            {
                Id = image.Id,
                FileName= image.FileName,
                FilePath =  image.DirectoryName,
                FileSizeMb = image.FileSizeInMB,
                FileType = image.FileType,
                SystemCreationTime = image.SystemCreationTime
            };
            libraryFolderContentsDTO.Files.Add(imageCardDTO);
        }
        libraryFolderContentsDTO.Files = _libraryBusinessFileService.GetImageCardImageData(libraryFolderContentsDTO.Files);
        
        foreach (var album in folder.ChildFolders)
        {
            LibraryAlbumDTO albumDto = new LibraryAlbumDTO();
            albumDto.Id = album.Id;
            albumDto.AlbumName = album.FolderName;
            
            if (album.FolderLevel == 0)
            {
                albumDto.AlbumPath = album.FolderPath;
            }
            else
            {
                albumDto.AlbumPath = album.FolderPath;
            }

            libraryFolderContentsDTO.Folders.Add(albumDto);
        }

        return libraryFolderContentsDTO;
    }

    public List<LibraryAlbumDTO> GetAllFolders()
    {
        var albums = _libraryService.GetAllLibraryFolders();
        List<LibraryAlbumDTO> albumDTOs = new List<LibraryAlbumDTO>();
        foreach (var album in albums)
        {
            LibraryAlbumDTO albumDto = new LibraryAlbumDTO();
            albumDto.Id = album.Id;
            albumDto.AlbumName = album.FolderName;
            
            if (album.FolderLevel == 0)
            {
                albumDto.AlbumPath = "Root Library Folder";
            }
            else
            {
                albumDto.AlbumPath = _applicationConfigurationSettings.TopLevelFolderName + "/" + album.FolderPath;
            }

            albumDTOs.Add(albumDto);
        }
        return albumDTOs;
    }
    
    
    public List<LibraryImageDTO> GetLibraryFolderImageContent(int id)
    {
        var album = _libraryService.GetLibraryFolderWithChildren(id);
        List<LibraryImageDTO> imageCardDTOs = new List<LibraryImageDTO>();
        
        foreach (var image in album.LibraryFiles)
        {
            var imageCardDTO = new LibraryImageDTO
            {
                Id = image.Id,
                FileName= image.FileName,
                FilePath =  image.DirectoryName,
                FileSizeMb = image.FileSizeInMB,
                FileType = image.FileType,
                SystemCreationTime = image.SystemCreationTime
            };
            imageCardDTOs.Add(imageCardDTO);
        }
        imageCardDTOs = _libraryBusinessFileService.GetImageCardImageData(imageCardDTOs);
        
        
        return imageCardDTOs;
    }
    
    public string GetRootFolderPath()
    {
        return _applicationConfigurationSettings.RootFolderPath;
    }
    
}