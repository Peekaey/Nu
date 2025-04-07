using Nu_Models.DTOs;

namespace Nu_Models.Results;

public class FolderReaderServiceResult
{
    public bool Success { get; set; }
    public string Error { get; set; }
    public List<FolderDTO> Folders { get; set; }
    public  FolderReaderServiceResult(bool isSuccess, string? errorMessage = null)
    {
        Success = isSuccess;
        Error = errorMessage;
    }

    public static  FolderReaderServiceResult AsSuccess(List<FolderDTO>folders)
    {
        return new  FolderReaderServiceResult(true)
        {
            Folders = folders
        };
    }
    
    public static  FolderReaderServiceResult AsFailure(string errorMessage)
    {
        return new  FolderReaderServiceResult(false, errorMessage);
    }
}