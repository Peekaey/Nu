using Nu_Models.DTOs;

namespace Nu_Models.Results;

public class PreviewThumbnailGenerateResult
{
    public bool Success { get; set; }
    public string Error { get; set; }
    public List<PreviewThumbnailDTO> Files { get; set; }
    public  PreviewThumbnailGenerateResult(bool isSuccess, string? errorMessage = null)
    {
        Success = isSuccess;
        Error = errorMessage;
    }

    public static  PreviewThumbnailGenerateResult AsSuccess(List<PreviewThumbnailDTO>files)
    {
        return new PreviewThumbnailGenerateResult(true)
        {
            Files = files
        };
    }
    
    public static  PreviewThumbnailGenerateResult AsFailure(string errorMessage)
    {
        return new  PreviewThumbnailGenerateResult(false, errorMessage);
    }
}