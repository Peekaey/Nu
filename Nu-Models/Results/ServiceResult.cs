namespace Nu_Models.Results;

public class ServiceResult
{
    public bool Success { get; set; }
    public string Error { get; set; }
    public ServiceResult(bool isSuccess, string? errorMessage = null)
    {
        Success = isSuccess;
        Error = errorMessage;
    }

    public static ServiceResult AsSuccess()
    {
        return new ServiceResult(true);
    }
    
    public static ServiceResult AsFailure(string errorMessage)
    {
        return new ServiceResult(false, errorMessage);
    }
}