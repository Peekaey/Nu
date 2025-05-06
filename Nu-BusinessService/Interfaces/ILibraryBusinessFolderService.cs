using Nu_Models;
using Nu_Models.DatabaseModels;
using Nu_Models.DTOs;
using Nu_Models.Results.BusinessServiceResults;

namespace Nu_BusinessService.Interfaces;

public interface ILibraryBusinessFolderService
{
    BusinessServiceResult<LibraryFolderContentsDTO?> GetLibraryRootContents();
    BusinessServiceResult<LibraryFolderContentsDTO?> GetLibraryFolderContents(int id);
    BusinessServiceResult<List<LibraryAlbumDTO>> GetAllFolders();
}