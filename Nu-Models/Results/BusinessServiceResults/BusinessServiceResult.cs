namespace Nu_Models.Results.BusinessServiceResults;

public class BusinessServiceResult<T>
{
    public bool Success { get; set; }
    public T? Data { get; set; }
    public string? ErrorMessage { get; set; }
    public int StatusCode { get; set; }
    
    private BusinessServiceResult(bool success, T? data, string? error, int statusCode)
    {
        Success = success;
        Data = data;
        ErrorMessage = error;
        StatusCode = statusCode;
    }

    public static BusinessServiceResult<T> Ok(T data) => 
        new(true, data, null, 200);

    public static BusinessServiceResult<T> NotFound(string error = "Item was not found") => 
        new(false, default, error, 404);

    public static BusinessServiceResult<T> Error(string error) => 
        new(false, default, error, 500);
}