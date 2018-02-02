using System.Collections.ObjectModel;
using MyMemory.Domain.Abstract;


namespace MyMemory.View
{

    public class Directories : ObservableCollection<DirectoryViewModel>, IPlaylistContainerSaver
    {

        internal DirectoryViewModel Add(string dirTitle, string dirPath)
        {
            var dir = new DirectoryViewModel { Name = dirTitle, Path = dirPath };

            Add(dir);

            return dir;
        }

        public void Save(IPlaylistContainerState state)
        {
            Clear();

            if (state?.Items != null)
            {
                foreach (var item in state.Items)
                {
                    Add(item.Name, item.Path);
                }
            }
        }
    }
}
