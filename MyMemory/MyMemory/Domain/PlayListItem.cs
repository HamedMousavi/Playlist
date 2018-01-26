namespace MyMemory.Domain
{

    public class PlayListItem : IPlayListItem
    {

        private readonly string _name;
        private readonly IPlayListItemPlayer _player;


        public PlayListItem(string name, IPlayListItemPlayer player)
        {
            _name = name;
            _player = player;
        }


        public void Play()
        {
            _player.Play(_name);
        }


        public override string ToString()
        {
            return _name;
        }
    }
}