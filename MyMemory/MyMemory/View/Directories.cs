using System.Collections.ObjectModel;
using MyMemory.Domain;


namespace MyMemory.View
{

    public class Directories : ObservableCollection<DirectoryViewModel>, IDirListSaver
    {

        internal DirectoryViewModel Add(string dirTitle, string dirPath)
        {
            var dir = new DirectoryViewModel { Name = dirTitle, Path = dirPath };

            Add(dir);

            return dir;
        }

        public void Save(IDirListState state)
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
