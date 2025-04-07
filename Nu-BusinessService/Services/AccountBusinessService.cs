using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Nu_BusinessService.Helpers;
using Nu_BusinessService.Interfaces;
using Nu_DataService.Interfaces;
using Nu_Models;
using Nu_Models.DatabaseModels;
using Nu_Models.DTOs;
using Nu_Models.Enums;
using Nu_Models.Extensions;
using Nu_Models.Results;
using SameSiteMode = Microsoft.AspNetCore.Http.SameSiteMode;

namespace Nu_BusinessService.Services;

public class AccountBusinessService : IAccountBusinessService
{
    private readonly ILogger<AccountBusinessService> _logger;
    private readonly IAccountService _accountService;
    private readonly JwtConfig _jwtConfig;
    
    public AccountBusinessService(ILogger<AccountBusinessService> logger, IAccountService accountService, JwtConfig jwtConfig)
    {
        _logger = logger;
        _accountService = accountService;
        _jwtConfig = jwtConfig;
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
    }
    
    public bool DoesAccountExist(string username)
    {
        var account = _accountService.GetAccountByUsername(username);
        
        if (account == null)
        {
            return false;
        }

        return true;
    }
    
    public AuthenticationResult AuthenticateUser(LoginRequest request)
    {
        var account = _accountService.GetAccountByUsername(request.Email);

        if (account == null || !Argon2PasswordHelper.VerifyPassword(request.Password, account.PasswordHash))
        {
            return AuthenticationResult.AsAuthenticationFailure();
        }
        
        // Generate JwtToken
        var jwtToken = JwtHelper.GenerateJwToken(account.Id,account.UserName, _jwtConfig.Secret);
        return AuthenticationResult.AsAuthenticatedSuccessfully(jwtToken);
    }
    
}