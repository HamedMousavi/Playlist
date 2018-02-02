using System;


namespace MyMemory.Domain
{

    public class PlaylistItem : IPlaylistItem
    {

        private readonly IPlaylistItemPlayer _player;


        public PlaylistItem()
        { }


        public PlaylistItem(IPlaylistItemPlayer player, string id, string name, string path)
        {
            Name = name;
            Path = path;
            Id = id;

            _player = player;
        }


        public void Play()
        {
            _player.Play(Path);
        }


        public override string ToString()
        {
            return Name;
        }


        public bool IsEqual(string id)
        {
            return string.Equals(Id, id, StringComparison.InvariantCultureIgnoreCase);
        }


        public string Name { get; set; }
        public string Path { get; set; }
        public string Id { get; set; }
    }
}