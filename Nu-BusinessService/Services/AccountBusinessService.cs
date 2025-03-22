using Microsoft.AspNetCore.Identity.Data;
using Microsoft.Extensions.Logging;
using Nu_BusinessService.Helpers;
using Nu_BusinessService.Interfaces;
using Nu_DataService.Interfaces;
using Nu_Models;
using Nu_Models.DatabaseModels;
using Nu_Models.DTOs;
using Nu_Models.Enums;
using Nu_Models.Results;

namespace Nu_BusinessService.Services;

public class AccountBusinessService : IAccountBusinessService
{
    private readonly ILogger<AccountBusinessService> _logger;
    private readonly IAccountService _accountService;
    
    public AccountBusinessService(ILogger<AccountBusinessService> logger, IAccountService accountService)
    {
        _logger = logger;
        _accountService = accountService;
    }
    
    public ServiceResult RegisterNewUser(RegisterRequest request)
    {
        var account = new Account
        {
            UserName = request.Email,
            PasswordHash = Argon2PasswordHelper.HashPassword(request.Password),
            AccountType = AccountType.Admin,
            IsActive = true,
        };

        var userProfile = new UserProfile
        {
            DisplayName = request.Email,
            Account = account
        };

        CreateNewUserDTO createNewUserDto = new CreateNewUserDTO
        {
            Account = account,
            UserProfile = userProfile
        };

        AuditableExtensions.InitialiseAuditableFields(createNewUserDto.Account);
        AuditableExtensions.InitialiseAuditableFields(createNewUserDto.UserProfile);
        
        var saveResult = _accountService.RegisterNewUser(createNewUserDto);
        
        if (!saveResult.Success)
        {
            return ServiceResult.AsFailure(saveResult.Error);
        }
        
        return ServiceResult.AsSuccess();
        
        
        
        return ServiceResult.AsSuccess();
    }
}