using Nu_Models.DatabaseModels;
using Nu_Models.DTOs;
using Nu_Models.Results;

namespace Nu_DataService.Interfaces;

public interface IAccountService
{
    ServiceResult RegisterNewUser(CreateNewUserDTO createNewUserDto);
    Account? GetAccountByUsername(string username);
}