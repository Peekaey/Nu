namespace Nu_Models.DatabaseModels;

public class UserProfilePicture
{
    public int Id { get; set; }
    public required string FileName { get; set; }
    public required string MimeType { get; set; }
    public required byte[] ImageData { get; set; }
    public DateTime CreatedDate { get; set; }
}