
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Nu_Apis.Interfaces;
using Nu_BusinessService.Interfaces;

namespace Nu_Apis.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class FolderController :ControllerBase
{
    private readonly ILogger<FolderController> _logger;
    private readonly IApiRequestValidationHelpers _apiRequestValidationHelpers;
    private readonly ILibraryBusinessFolderService _libraryBusinessFolderService;
    ILibraryBusinessFileService _libraryBusinessFileService;
    
    public FolderController(ILogger<FolderController> logger, IApiRequestValidationHelpers apiRequestValidationHelpers, 
        ILibraryBusinessFolderService libraryBusinessFolderService, ILibraryBusinessFileService libraryBusinessFileService)
    {
        _logger = logger;
        _apiRequestValidationHelpers = apiRequestValidationHelpers;
        _libraryBusinessFolderService = libraryBusinessFolderService;
        _libraryBusinessFileService = libraryBusinessFileService;
    }
    
    // TODO Add Endpoint Verification
    [HttpGet("all", Name = "all")]
    public IActionResult GetLibraryFoldersAll()
    {
        var libraryFolders = _libraryBusinessFolderService.GetAllFolders();
        if (!libraryFolders.Success)
        {
            if (libraryFolders.StatusCode == 404)
            {
                return NotFound(libraryFolders.ErrorMessage);
            }
            else
            {
                return StatusCode(500, "Internal error retrieving folders");
            }
        }
        return Ok(libraryFolders.Data);
    }

    [HttpGet("root", Name = "root")]
    public IActionResult GetLibraryRootContents()
    {
        var libraryFolders = _libraryBusinessFolderService.GetLibraryRootContents();
        
        if (!libraryFolders.Success)
        {
            if (libraryFolders.StatusCode == 404)
            {
                return NotFound(libraryFolders.ErrorMessage);
            }
            else
            {
                return StatusCode(500, "Internal error retrieving folders");
            }
        }
        return Ok(libraryFolders.Data);
        
    }

    [HttpGet("album/{id}")]
    public IActionResult GetLibraryAlbumContent(int id)
    {
        var libraryAlbum = _libraryBusinessFileService.GetLibraryFolderImageContent(id);
        
        if (!libraryAlbum.Success)
        {
            if (libraryAlbum.StatusCode == 404)
            {
                return NotFound(libraryAlbum.ErrorMessage);
            }
            else
            {
                return StatusCode(500, "Internal error retrieving folders");
            }
        }
        return Ok(libraryAlbum.Data);
    }

    [HttpGet("folder/{id}")]
    public IActionResult GetLibraryFolderContent(int id)
    {
        var libraryFolder = _libraryBusinessFolderService.GetLibraryFolderContents(id);
        
        if (!libraryFolder.Success)
        {
            if (libraryFolder.StatusCode == 404)
            {
                return NotFound(libraryFolder.ErrorMessage);
            }
            else
            {
                return StatusCode(500, "Internal error retrieving folders");
            }
        }
        
        return Ok(libraryFolder.Data);
    }
    
    
    
    
    
}