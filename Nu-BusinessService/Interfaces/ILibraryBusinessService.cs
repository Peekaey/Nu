using Microsoft.AspNetCore.Mvc;
using Nu_Models;
using Nu_Models.DTOs;

namespace Nu_BusinessService.Interfaces;

public interface ILibraryBusinessService
{
    List<LibraryAlbumDTO> GetAllFolders();
    List<LibraryImageDTO> GetLibraryFolderImageContent(int id);
    LibraryFolderContentsDTO GetLibraryRootContents();
    LibraryFolderContentsDTO GetLibraryFolderContents(int id);
    string GetRootFolderPath();
    string GetImageById(int id);
    FileStreamResult DownloadFile(int id);
}