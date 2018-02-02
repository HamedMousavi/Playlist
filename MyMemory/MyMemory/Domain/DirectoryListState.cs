using System.Collections.Generic;
using MyMemory.Domain.Abstract;

namespace MyMemory.Domain
{
    public class DirectoryListState : IPlaylistContainerState
    {

        public DirectoryListState(List<DirectoryItem> directories)
        {
            Items = directories;
        }

        public List<DirectoryItem> Items { get; set; }
    }
}