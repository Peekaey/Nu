using Nu_Models.DatabaseModels;

namespace Nu_Models.DTOs;

public class CreateNewUserDTO
{
    public Account Account { get; set; }
    public UserProfile UserProfile { get; set; }
}