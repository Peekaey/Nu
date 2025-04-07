namespace Nu_Models.Extensions;

public static class AuditableExtensions
{
    public static void SetCreatedDate(this IAuditable auditable)
    {
        auditable.CreatedDate = DateTime.UtcNow;
        auditable.LastUpdatedDate = DateTime.UtcNow;
    }
    public static void SetUpdatedDate(this IAuditable auditable)
    {
        auditable.LastUpdatedDate = DateTime.UtcNow;
    }
    
    public static void InitialiseAuditableFields<T>(T entity) where T : IAuditable
    {
        entity.SetCreatedDate();
        entity.SetUpdatedDate();
    }
}