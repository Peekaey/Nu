﻿using Microsoft.Extensions.Logging;
using Nu_DataService.Interfaces;
using Nu_Models.DatabaseModels;
using Nu_Models.DTOs;
using Nu_Models.Results;

namespace Nu_DataService.Services;

public class AccountService : IAccountService
{
    private readonly ILogger<AccountService> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public AccountService(ILogger<AccountService> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public ServiceResult RegisterNewUser(CreateNewUserDTO createNewUserDto)
    {
        _unitOfWork.AccountRepository.Add(createNewUserDto.Account);
        _unitOfWork.UserProfileRepository.Add(createNewUserDto.UserProfile);

        try
        {
            _unitOfWork.SaveChanges();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error saving new user");
            return ServiceResult.AsFailure("Error saving new user");
        }
        
        return ServiceResult.AsSuccess();
    }
    
    public Account? GetAccountByUsername(string username)
    {
        return _unitOfWork.AccountRepository.GetByAccountUsername(username);
    }
}