namespace MyMemory.Domain
{

    public class PlaylistItem : IPlaylistItem
    {

        private readonly IPlaylistItemPlayer _player;


        public PlaylistItem(string name, IPlaylistItemPlayer player, string resourcePath)
        {
            Name = name;
            _player = player;
            Path = resourcePath;
        }


        public void Play()
        {
            _player.Play(Path);
        }


        public override string ToString()
        {
            return Name;
        }


        public string Name { get; set; }
        public string Path { get; set; }
    }
}