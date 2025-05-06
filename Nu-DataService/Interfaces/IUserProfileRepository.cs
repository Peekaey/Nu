using Nu_Models.DatabaseModels;

namespace Nu_DataService.Interfaces;

public interface IUserProfileRepository
{
    Task AddAsync(UserProfile userProfile);
    
    Task<UserProfile?> GetByIdAsync(int id);
    void Add(UserProfile userProfile);
    void Remove(UserProfile userProfile);
    void Update(UserProfile userProfile);
    UserProfile? GetById(int id);
}