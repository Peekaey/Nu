using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nu_Apis.Interfaces;
using Nu_BusinessService.Interfaces;

namespace Nu_Apis.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class FolderController :ControllerBase
{
    private readonly ILogger<AuthController> _logger;
    private readonly IApiRequestValidationHelpers _apiRequestValidationHelpers;
    private readonly ILibraryBusinessService _libraryBusinessService;
    
    public FolderController(ILogger<AuthController> logger, IApiRequestValidationHelpers apiRequestValidationHelpers,
        ILibraryBusinessService libraryBusinessService)
    {
        _logger = logger;
        _apiRequestValidationHelpers = apiRequestValidationHelpers;
        _libraryBusinessService = libraryBusinessService;
    }

    [HttpGet("all", Name = "all")]
    public IActionResult GetLibraryFoldersAll()
    {
        var libraryFolders = _libraryBusinessService.GetAllFolders();
        return Ok(libraryFolders);
    }

    [HttpGet("root", Name = "root")]
    public IActionResult GetLibraryRootContents()
    {
        var libraryFolders = _libraryBusinessService.GetLibraryRootContents();
        return Ok(libraryFolders);
        
    }

    [HttpGet("album/{id}")]
    public IActionResult GetLibraryAlbumContent(int id)
    {
        var libraryAlbum = _libraryBusinessService.GetLibraryFolderImageContent(id);
        return Ok(libraryAlbum);
    }

    [HttpGet("folder/{id}")]
    public IActionResult GetLibraryFolderContent(int id)
    {
        var libraryFolder = _libraryBusinessService.GetLibraryFolderContents(id);
        Debug.WriteLine("Library Folder: " + libraryFolder);
        return Ok(libraryFolder);
    }
    
    
    
    
    
}