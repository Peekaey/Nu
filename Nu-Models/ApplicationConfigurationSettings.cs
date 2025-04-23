namespace Nu_Models;

public class ApplicationConfigurationSettings
{
    public string RootFolderPath { get; set; }

    public string TopLevelFolderName => new DirectoryInfo(RootFolderPath).Name;

    public string FolderPathWithoutTopLevelFolder
    {
        get
        {
            var rootFolderPath = RootFolderPath.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            var replacedRootFolderPath = rootFolderPath.Replace("\\", "/");
            int lastSlashIndex = replacedRootFolderPath.LastIndexOf('/');
            if (lastSlashIndex < 0)
            {
                return replacedRootFolderPath;
            }
            else
            {
                return rootFolderPath.Substring(0, lastSlashIndex + 1);
                
            }

        }
    }

    public readonly HashSet<string> AcceptedFileTypes = new()
    {
        ".jpg",
        ".jpeg",
        ".png",
        ".gif",
        ".jfif",
        ".webp",
    };
}