using System.Collections.Generic;
using System.Linq;
using MyMemory.Domain.Abstract;


namespace MyMemory.Domain
{

    public class PlaylistState : IPlaylistState
    {

        public PlaylistState() : this(null, string.Empty) { }


        public PlaylistState(IEnumerable<FileResource> resources) : this(resources, string.Empty) { }


        public PlaylistState(IEnumerable<IResource> resources, string selectedItemId)
        {
            Resources = resources;
            SelectedItemId = selectedItemId;
        }


        public bool IsEmpty => Resources == null || !Resources.Any();

        public string SelectedItemId { get; set; }

        public IEnumerable<IResource> Resources { get; set; }
    }
}
