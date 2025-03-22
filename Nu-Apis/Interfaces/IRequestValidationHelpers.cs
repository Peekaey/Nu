using Microsoft.AspNetCore.Identity.Data;

namespace Nu_Apis.Interfaces;

public interface IRequestValidationHelpers
{
    bool ValidateRegisterRequest(RegisterRequest request);
}