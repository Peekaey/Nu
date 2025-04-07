using Nu_Models.DTOs;

namespace Nu_Models.Results;

public class FileReaderServiceResult
{
    public bool Success { get; set; }
    public string Error { get; set; }
    public List<FileDTO> Files { get; set; }
    public  FileReaderServiceResult(bool isSuccess, string? errorMessage = null)
    {
        Success = isSuccess;
        Error = errorMessage;
    }

    public static  FileReaderServiceResult AsSuccess(List<FileDTO>files)
    {
        return new  FileReaderServiceResult(true)
        {
            Files = files
        };
    }
    
    public static  FileReaderServiceResult AsFailure(string errorMessage)
    {
        return new  FileReaderServiceResult(false, errorMessage);
    }
}