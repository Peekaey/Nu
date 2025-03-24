using Microsoft.AspNetCore.Identity.Data;
using Nu_Models.Results;

namespace Nu_BusinessService.Interfaces;

public interface IAccountBusinessService
{
    ServiceResult RegisterNewUser(RegisterRequest request);
    AuthenticationResult AuthenticateUser(LoginRequest request);
    bool DoesAccountExist(string username);
}