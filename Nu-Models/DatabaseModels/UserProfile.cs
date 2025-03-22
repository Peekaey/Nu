namespace Nu_Models.DatabaseModels;

public class UserProfile : IAuditable
{
    public int Id { get; set; }
    // Display Name
    public required string DisplayName { get; set; }
    
    public DateTime CreatedDate { get; set; }
    public DateTime LastUpdatedDate { get; set; }
    
    public int? UserProfilePictureId { get; set; }
    public UserProfilePicture? UserProfilePicture { get; set; }
    
    public int AccountId { get; set; }
    public required Account Account { get; set; }
}