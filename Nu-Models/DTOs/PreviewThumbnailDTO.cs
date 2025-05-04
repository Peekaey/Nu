namespace Nu_Models.DTOs;

public class PreviewThumbnailDTO
{
    public DateTime CreationTime { get; set; }
    public string DirectoryName { get; set; }
    public string FileType { get; set; }
    public string FullName { get; set; }
    public string FileName { get; set; }
    public long FileSize { get; set; }
    public double FileSizeInMB => FileSize / (1024.0 * 1024.0);
}