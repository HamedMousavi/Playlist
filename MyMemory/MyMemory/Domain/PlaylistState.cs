using System.Collections.Generic;
using System.Linq;


namespace MyMemory.Domain
{

    public class PlaylistState : IPlaylistState
    {

        public PlaylistState() : this(null, 0) { }


        public PlaylistState(IEnumerable<FileResource> resources) : this(resources, 0) { }


        public PlaylistState(IEnumerable<INameableResource> resources, int index)
        {
            Resources = resources;
            Index = index;
        }


        public bool IsEmpty => Resources == null || !Resources.Any();
        public int Index { get; set; }

        public IEnumerable<INameableResource> Resources { get; set; }
    }
}
