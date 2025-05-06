using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nu_DataService.Interfaces;
using Nu_Models.DatabaseModels;

namespace Nu_DataService.Repositories;

public class UserProfileRepository : IUserProfileRepository
{
    private readonly DataContext _context;
    private readonly ILogger<UserProfileRepository> _logger;
    
    public UserProfileRepository(DataContext context, ILogger<UserProfileRepository> logger)
    {
        _context = context;
        _logger = logger;
    }
    
    public async Task AddAsync(UserProfile userProfile)
    {
        await _context.UserProfiles.AddAsync(userProfile);
    }
    
    public void Add(UserProfile userProfile)
    {
        _context.UserProfiles.Add(userProfile);
    }
    
    public void Remove(UserProfile userProfile)
    {
        _context.UserProfiles.Remove(userProfile);
    }
    
    
    public void Update(UserProfile userProfile)
    {
        _context.UserProfiles.Update(userProfile);
    }
    
    public async Task<UserProfile?> GetByIdAsync(int id)
    {
        return await _context.UserProfiles.FirstOrDefaultAsync(x => x.Id == id);
    }
    
    public UserProfile? GetById(int id)
    {
        return _context.UserProfiles.FirstOrDefault(x => x.Id == id);
    }
}