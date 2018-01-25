// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileAttachment.cs" company="www.OrderedSoft.com">
//   Author: Hamed Mousavi: HamedMosavi[at] Yahoo (dot) com
//   License agreement: Please read License.txt provided in solution directory
// </copyright>
// <summary>
//   Defines the FileAttachment type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


namespace HLib.Network.Email
{

    using System.Windows.Media.Imaging;

    using HLib.Io;


    public class FileAttachment
    {
        
        private string _path;


        public bool IsIncluded { get; set; }


        public string FilePath
        {
            get
            {
                return _path;
            }
            set
            {
                _path = value;
                Name = string.IsNullOrWhiteSpace(_path) ? string.Empty : System.IO.Path.GetFileName(_path);
            }
        }


        public string Name { get; private set; }
        

        public BitmapSource Icon
        {
            get
            {
                return IconExtractor.Instance.ExtractByFilePath(FilePath);
            }
        }


        public override string ToString()
        {
            return Name;
        }
    }
}
