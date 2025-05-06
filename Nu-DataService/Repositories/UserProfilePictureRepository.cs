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
    
    public async Task AddAsync(UserProfilePicture userProfilePicture)
    {
        await _context.UserProfilePictures.AddAsync(userProfilePicture);
    }
    
    public void Add(UserProfilePicture userProfilePicture)
    {
        _context.UserProfilePictures.Add(userProfilePicture);
    }
    
    public void Remove(UserProfilePicture userProfilePicture)
    {
        _context.UserProfilePictures.Remove(userProfilePicture);
    }
    
    public void Update(UserProfilePicture userProfilePicture)
    {
        _context.UserProfilePictures.Update(userProfilePicture);
    }
    
    public async Task<UserProfilePicture?> GetAsync(int id)
    {
        return await _context.UserProfilePictures.FirstOrDefaultAsync(x => x.Id == id);
    }
    
    public UserProfilePicture? Get(int id)
    {
        return _context.UserProfilePictures.FirstOrDefault(x => x.Id == id);
    }
}