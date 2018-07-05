using System;


namespace MyMemory.Domain
{

    public class DirectoryItem : IComparable<DirectoryItem>
    {

        public DirectoryItem(string dirTitle, string dirPath)
        {
            Name = dirTitle;
            Path = dirPath;
        }


        public string Name { get; set; }
        public string Path { get; set; }


        public int CompareTo(DirectoryItem other)
        {
            return string.Compare(this.Name, other.Name, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}