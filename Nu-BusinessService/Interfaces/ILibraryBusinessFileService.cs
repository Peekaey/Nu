using Microsoft.AspNetCore.Mvc;
using Nu_Models;
using Nu_Models.DTOs;
using Nu_Models.Results.BusinessServiceResults;

namespace Nu_BusinessService.Interfaces;

public interface ILibraryBusinessFileService
{
    List<LibraryImageDTO> GetImageCardImageData(List<LibraryImageDTO> libraryFiles);
    BusinessServiceResult<List<LibraryImageDTO>> GetLibraryFolderImageContent(int id);
    BusinessServiceResult<string?> GetImagePathById(int id);
    BusinessServiceResult<FileStreamResult?> DownloadFile(int id);
}