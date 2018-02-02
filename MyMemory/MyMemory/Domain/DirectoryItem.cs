namespace MyMemory.Domain
{
    public class DirectoryItem
    { 

        public DirectoryItem(string dirTitle, string dirPath)
        {
            Name = dirTitle;
            Path = dirPath;
        }

        public string Name { get; set; }
        public string Path { get; set; }
    }
}