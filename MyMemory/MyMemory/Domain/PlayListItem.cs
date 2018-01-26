namespace MyMemory.Domain
{

    public class PlayListItem : IPlayListItem
    {

        private readonly string _filePath;
        private readonly string _name;
        private readonly IPlayListItemPlayer _player;


        public PlayListItem(string name, IPlayListItemPlayer player, string filePath)
        {
            _name = name;
            _player = player;
            _filePath = filePath;
        }


        public void Play()
        {
            _player.Play(_filePath);
        }


        public override string ToString()
        {
            return _name;
        }
    }
}