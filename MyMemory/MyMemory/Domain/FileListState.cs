using System.Collections.Generic;
using System.Linq;


namespace MyMemory.Domain
{

    public class FileListState : IPlayListState
    {

        public FileListState(IEnumerable<string> fileNames, int index)
        {
            Names = fileNames;
            Index = index;
        }


        public bool IsEmpty => Names == null || !Names.Any();
        public IEnumerable<string> Names { get; set; }
        public int Index { get; set; }
    }
}
