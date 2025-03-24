using Microsoft.AspNetCore.Identity.Data;
using Nu_Apis.Interfaces;

namespace Nu_Apis.Helpers;

public class ApiApiRequestValidationHelpers : IApiRequestValidationHelpers
{
    // TODO Prevent SQL Injection
    public bool ValidateRegisterRequest(RegisterRequest request)
    {
        if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
        {
            return false;
        }
        
        if (!request.Email.Contains("@") || !request.Email.Contains("."))
        {
            return false;
        }

        return true;
    }
    
    public bool ValidateLoginRequest(LoginRequest request)
    {
        if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
        {
            return false;
        }
        
        if (!request.Email.Contains("@") || !request.Email.Contains("."))
        {
            return false;
        }
        
        return true;
    }
    
}