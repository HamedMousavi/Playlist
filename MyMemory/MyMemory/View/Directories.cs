using System.Collections.ObjectModel;
using MyMemory.Domain.Abstract;


namespace MyMemory.View
{

    public class Directories : ObservableCollection<DirectoryViewModel>, IPlaylistContainerSaver
    {

        public void Save(IPlaylistContainerState state)
        {
            Clear();
            if (state?.Items == null) return;

            foreach (var item in state.Items)
            {
                Add(CreateViewModel(item.Name, item.Path));
            }
        }


        public DirectoryViewModel CreateViewModel(string dirTitle, string dirPath)
        {
            return new DirectoryViewModel { Name = dirTitle, Path = dirPath };
        }
    }
}
