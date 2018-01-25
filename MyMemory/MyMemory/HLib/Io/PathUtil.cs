// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PathUtil.cs" company="www.OrderedSoft.com">
//   Author: Hamed Mousavi: HamedMosavi[at] Yahoo (dot) com
//   License agreement: Please read License.txt provided in solution directory
// </copyright>
// <summary>
//   Defines the PathUtil type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


namespace HLib.Io
{
    
    using System.IO;
    using System.Text.RegularExpressions;

    
    public class PathUtil
    {

        // I hope it's unique among all versions of Win OS
        private static int MAX_PATH = 259;


        private static readonly Regex _invalidFileOrPathNameRegex =
            new Regex(
                string.Format(
                    "[{0}]",
                    Regex.Escape(
                        new string(Path.GetInvalidFileNameChars()) +
                        "." +
                        new string(Path.GetInvalidPathChars()))));


        /// <summary>
        /// Gets startup path of current running assembly
        /// </summary>
        public static string ApplicationDirectory
        {
            get
            {
                return Path.GetDirectoryName(
                    System.Reflection.Assembly.GetExecutingAssembly().Location);
            }
        }


        /// <summary>
        /// Ensures given path is not null and exists
        /// </summary>
        /// <param name="path">Path to check</param>
        /// <returns>True if given path is to a directory</returns>
        public static bool DirectoryPathIsValid(string path)
        {
            return !string.IsNullOrWhiteSpace(path) && Directory.Exists(path) && path.Length < MAX_PATH;
        }


        public static string Normalize(string fileOrDirName)
        {
            // UNDONE: THIS IS FRIGHTENING!
            var path = _invalidFileOrPathNameRegex.Replace(fileOrDirName, string.Empty);
            return path.Substring(0, System.Math.Min(path.Length, MAX_PATH));
        }


        public static bool FilePathIsValid(string path)
        {
            return !string.IsNullOrWhiteSpace(path) && File.Exists(path) && path.Length < MAX_PATH;
        }


        public static string PathAndName(string path, string namePath)
        {
            try
            {
                if (DirectoryPathIsValid(path))
                {
                    var name = Path.GetFileName(namePath);

                    if (!string.IsNullOrWhiteSpace(name))
                    {
                        return Path.Combine(path, name);
                    }
                }
            }
            catch
            {

            }

            return string.Empty;
        }
    }
}
