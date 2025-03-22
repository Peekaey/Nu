using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nu_DataService.Interfaces;
using Nu_Models.DatabaseModels;

namespace Nu_DataService.Repositories;

public class UserProfilePictureRepository : IUserProfilePictureRepository
{
    private readonly DataContext _context;
    private readonly ILogger<UserProfilePictureRepository> _logger;
    
    public UserProfilePictureRepository(DataContext context, ILogger<UserProfilePictureRepository> logger)
    {
        _context = context;
        _logger = logger;
    }
    
    public async Task<UserProfilePicture> AddAsync(UserProfilePicture userProfilePicture)
    {
        await _context.UserProfilePictures.AddAsync(userProfilePicture);
        await _context.SaveChangesAsync();
        return userProfilePicture;
    }
    
    public void Add(UserProfilePicture userProfilePicture)
    {
        _context.UserProfilePictures.Add(userProfilePicture);
    }
    
    public async Task<UserProfilePicture> RemoveAsync(UserProfilePicture userProfilePicture)
    {
        _context.UserProfilePictures.Remove(userProfilePicture);
        await _context.SaveChangesAsync();
        return userProfilePicture;
    }
    
    public void Remove(UserProfilePicture userProfilePicture)
    {
        _context.UserProfilePictures.Remove(userProfilePicture);
    }
    
    public async Task<UserProfilePicture> UpdateAsync(UserProfilePicture userProfilePicture)
    {
        _context.UserProfilePictures.Update(userProfilePicture);
        await _context.SaveChangesAsync();
        return userProfilePicture;
    }
    
    public void Update(UserProfilePicture userProfilePicture)
    {
        _context.UserProfilePictures.Update(userProfilePicture);
    }
    
    public async Task<UserProfilePicture> GetAsync(int id)
    {
        return await _context.UserProfilePictures.FirstOrDefaultAsync(x => x.Id == id);
    }
    
    public UserProfilePicture Get(int id)
    {
        return _context.UserProfilePictures.FirstOrDefault(x => x.Id == id);
    }
}