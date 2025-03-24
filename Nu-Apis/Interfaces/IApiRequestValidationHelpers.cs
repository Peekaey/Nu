using Microsoft.AspNetCore.Identity.Data;

namespace Nu_Apis.Interfaces;

public interface IApiRequestValidationHelpers
{
    bool ValidateRegisterRequest(RegisterRequest request);
    bool ValidateLoginRequest(LoginRequest request);
}