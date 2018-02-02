using System.Collections.Generic;
using System.Linq;


namespace MyMemory.Domain
{

    public class PlaylistState : IPlaylistState
    {

        public PlaylistState() : this(null, string.Empty) { }


        public PlaylistState(IEnumerable<FileResource> resources) : this(resources, string.Empty) { }


        public PlaylistState(IEnumerable<INameableResource> resources, string selectedItemId)
        {
            Resources = resources;
            SelectedItemId = selectedItemId;
        }


        public bool IsEmpty => Resources == null || !Resources.Any();

        public string SelectedItemId { get; set; }

        public IEnumerable<INameableResource> Resources { get; set; }
    }
}
