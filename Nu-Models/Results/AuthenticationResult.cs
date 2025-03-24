using Nu_Models.Enums;

namespace Nu_Models.Results;

public class AuthenticationResult
{
    public AuthenticationResultType AuthenticationResultType { get; set; }
    public string? Cookie { get; set; }
    public string? Error { get; set; }
    public AuthenticationResult(AuthenticationResultType authenticationResultType, string? cookie = null, string? errorMessage = null)
    {
        AuthenticationResultType = authenticationResultType;
        Error = errorMessage;
        Cookie = cookie;
    }

    public static AuthenticationResult AsAuthenticatedSuccessfully(string jwToken)
    {
        return new AuthenticationResult(AuthenticationResultType.Success, jwToken);
    }

    public static AuthenticationResult AsAuthenticationFailure()
    {
        return new AuthenticationResult(AuthenticationResultType.InvalidCredentials);
    }

    public static AuthenticationResult AsRequestFailure(string errorMessage)
    {
        return new AuthenticationResult(AuthenticationResultType.RequestFailure, null, errorMessage);
    }
}