using Nu_Models.DatabaseModels;

namespace Nu_DataService.Interfaces;

public interface IUserProfilePictureRepository
{
    Task<UserProfilePicture> AddAsync(UserProfilePicture userProfilePicture);
    Task<UserProfilePicture> RemoveAsync(UserProfilePicture userProfilePicture);
    Task<UserProfilePicture> UpdateAsync(UserProfilePicture userProfilePicture);
    Task<UserProfilePicture> GetAsync(int id);
    void Add(UserProfilePicture userProfilePicture);
    void Remove(UserProfilePicture userProfilePicture);
    void Update(UserProfilePicture userProfilePicture);
    UserProfilePicture Get(int id);
    
}