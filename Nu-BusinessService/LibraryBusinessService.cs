using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nu_BusinessService.Interfaces;
using Nu_DataService.Interfaces;
using Nu_Models;
using Nu_Models.DatabaseModels;
using Nu_Models.DTOs;
using Nu_Models.Extensions;
using Nu_Models.Extensions.Interfaces;

namespace Nu_BusinessService;

// TODO Clean up spaghetti code and organise into separate services
public class LibraryBusinessService : ILibraryBusinessService
{
    private readonly ILogger<LibraryBusinessService> _logger;
    private readonly ILibraryService _libraryService;
    private readonly ApplicationConfigurationSettings _applicationConfigurationSettings;
    private readonly ILibraryBusinessFileService _libraryBusinessFileService;
    private readonly FilePathExtensions _filePathExtensions;
    private readonly IMappingHelpers _mappingHelpers;
    
    public LibraryBusinessService(ILogger<LibraryBusinessService> logger, ILibraryService libraryService, 
        ApplicationConfigurationSettings applicationConfigurationSettings, ILibraryBusinessFileService libraryBusinessFileService,
        FilePathExtensions filePathExtensions, IMappingHelpers mappingHelpers)
    {
        _applicationConfigurationSettings = applicationConfigurationSettings;
        _logger = logger;
        _libraryService = libraryService;
        _libraryBusinessFileService = libraryBusinessFileService;
        _filePathExtensions = filePathExtensions;
        _mappingHelpers = mappingHelpers;
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
                SystemCreationTime = image.SystemCreationTime,
                PreviewThumbNailId = image.LibraryPreviewThumbnailIndexId ?? null
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
        libraryFolderContentsDTO.FolderPath = _mappingHelpers.MapFolderPathToChunks(folder.FolderPath).ToList();
        libraryFolderContentsDTO.FolderPath = _libraryService.GetLibraryFolderPathChunkIds(libraryFolderContentsDTO.FolderPath);
        foreach (var image in folder.LibraryFiles)
        {
            var imageCardDTO = new LibraryImageDTO
            {
                Id = image.Id,
                FileName= image.FileName,
                FilePath =  image.DirectoryName,
                FileSizeMb = image.FileSizeInMB,
                FileType = image.FileType,
                SystemCreationTime = image.SystemCreationTime,
                ServerImagePath = image.FullName, // Placeholder value to be corrected later
                PreviewThumbNailId = image.LibraryPreviewThumbnailIndexId ?? null
                
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
                SystemCreationTime = image.SystemCreationTime,
                PreviewThumbNailId = image.LibraryPreviewThumbnailIndexId ?? null

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

    public string GetImageById(int id)
    {
        var file = _libraryService.GetLibraryFileById(id);
        var imageData = string.Empty;
        // TODO Add nullable reference support
        if (file != null)
        {
            var fullFilePath = Path.Combine(_applicationConfigurationSettings.FolderPathWithoutTopLevelFolder, file.DirectoryName, file.FileName);
            imageData = _filePathExtensions.MapImageUrlToServerUrl(fullFilePath);
        }

        return imageData;
    }

    public FileStreamResult DownloadFile(int id)
    {
        var file = _libraryService.GetLibraryFileById(id);
        if (file == null)
        {
            throw new FileNotFoundException($"File with ID {id} not found");
        }

        var fullFilePath = Path.Combine(_applicationConfigurationSettings.FolderPathWithoutTopLevelFolder, file.DirectoryName, file.FileName);

        if (!System.IO.File.Exists(fullFilePath))
        {
            throw new FileNotFoundException($"File not found at path: {fullFilePath}");
        }

        var stream = new FileStream(fullFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        var mimeType = GetMimeType(Path.GetExtension(file.FileName));

        return new FileStreamResult(stream, mimeType)
        {
            FileDownloadName = file.FileName
        };
    }
    
    // Extract to external utility
    // Filter out non-supported file types
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