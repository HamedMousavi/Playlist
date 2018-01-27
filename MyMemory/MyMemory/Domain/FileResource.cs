namespace MyMemory.Domain
{
    public class FileResource : INameableResource
    {
        public FileResource(string path)
        {
            Path = path;
        }

        public string Name => string.IsNullOrWhiteSpace(Path) ? null : System.IO.Path.GetFileName(Path);
        public string Path { get; set; }
    }
}