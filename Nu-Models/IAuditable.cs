namespace Nu_Models;

public interface IAuditable
{
    public DateTime CreatedDate { get; set; }
    public DateTime LastUpdatedDate { get; set; }
}