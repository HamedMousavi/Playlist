// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DirectoryEx.cs" company="www.OrderedSoft.com">
//   Author: Hamed Mousavi: HamedMosavi[at] Yahoo (dot) com
//   License agreement: Please read License.txt provided in solution directory
// </copyright>
// <summary>
//   Defines the DirectoryEx type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


namespace HLib.Io
{

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;


    public class DirectoryEx
    {

        public static bool CopyContent(string source, string destination)
        {
            bool result = true;

            var dirInf = new DirectoryInfo(source);
            var files = dirInf.EnumerateFiles();
            {
                Parallel.ForEach(
                    files,
                    (file, state) =>
                        {
                            try
                            {
                                var index = 0;
                                var path = Path.Combine(destination, file.Name);
                                while (File.Exists(path))
                                {
                                    path = Path.Combine(destination, string.Format("Copy ({0}) of {1}", ++index, file.Name));
                                }

                                file.CopyTo(path, false);
                            }
                            catch (Exception)
                            {
                                result = false;
                                // Do not stop operation, maybe results will be good-enough for user
                                // Otherwise they'll delete results
                            }
                        });
            }

            return result;
        }


        public static bool Create(string path)
        {
            var result = true;
            try
            {
                // UNDONE: Make sure dir path is ok
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }
            catch (Exception exception)
            {
                result = false;
                Logging.Loggers.Null.LogException(exception);
            }

            return result;
        }


        /// <summary>
        /// Tries to copy content from source to destination.
        /// In case of failure operation will be aborted and
        /// will not be undone. (It's not atomic or transactional)
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        public static bool MoveContent(string source, string destination)
        {
            var res = true;
            if (CopyContent(source, destination))
            {
                res &= DeleteContent(source);
            }

            return res;
        }


        /// <summary>
        /// Tries to deleted files inside given path. Will NOT remove
        /// sub directories. If a file fails to remove, it will not be
        /// removed and process will continue anyway.
        /// </summary>
        /// <param name="source"></param>
        /// <returns>True if all files have been deleted, otherwise 0.</returns>
        private static bool DeleteContent(string source)
        {
            var result = true;

            var dirInf = new DirectoryInfo(source);
            var files = dirInf.GetFiles();
            {
                Parallel.ForEach(
                    files,
                    (file, state) =>
                    {
                        try
                        {
                            file.Delete();
                        }
                        catch (Exception)
                        {
                            result = false;
                            // Do not stop operation, maybe results will be good-enough for user
                            // Otherwise they'll delete results
                        }
                    });
            }

            return result;
        }


        public static bool IsDirectoryEmpty(string path, string filter)
        {
            if (!PathUtil.DirectoryPathIsValid(path)) return true;
           
            var dirInf = new DirectoryInfo(path);
            var files = dirInf.GetFiles(filter);
            {
                if (files.Length <= 0) return true;
            }

            return false;
        }


        /// <summary>
        /// Returns a list of all files in a directory.
        /// Doesn't enumerate subdirectories
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static IEnumerable<FileInfo> GetFiles(string path)
        {
            var dirInf = new DirectoryInfo(path);
            return dirInf.EnumerateFiles("*.*", SearchOption.TopDirectoryOnly);
        }


        /// <summary>
        /// Counts number of FILES in a directory.
        /// </summary>
        /// <param name="path">Path to directory</param>
        /// <param name="checkSubDirectories">True will search subdirectories for files too.</param>
        /// <returns>True if 0 files exists (even if directory exisst), false if any file exists.</returns>
        public static bool IsEmpty(string path, bool checkSubDirectories)
        {
            return GetFileCount(path, "*.*", checkSubDirectories) == 0;
        }



        public static int GetFileCount(string path, string filter, bool countSubDirectories)
        {
            var files = Directory.GetFiles(path, filter, countSubDirectories? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
            return files.Length;
        }
    }
}
