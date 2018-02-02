// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StandarWindowsDialogs.cs" company="www.OrderedSoft.com">
//   Author: Hamed Mousavi: HamedMosavi[at] Yahoo (dot) com
//   License agreement: Please read License.txt provided in solution directory
// </copyright>
// <summary>
//   Defines the StandarWindowsDialogs type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


namespace HLib.Gui
{

    public class StandarWindowsDialogs
    {

        private static string _lastSelectedFilePath;
        private static string _lastSelectedDirectoryPath;


        public static bool BrowseForFile(ref string path)
        {
            var res = false;

            var dlg = new Microsoft.Win32.OpenFileDialog();

            try
            {
                // Try selecting an initial directory for user
                if (!string.IsNullOrWhiteSpace(_lastSelectedFilePath))
                {
                    dlg.InitialDirectory = System.IO.Path.GetDirectoryName(_lastSelectedFilePath);
                }
            }
            catch
            {
                // Silently ignore exception, it won't affect normal operation
            }

            if (dlg.ShowDialog() == true)
            {
                _lastSelectedFilePath = dlg.FileName;
                path = dlg.FileName;
                res = true;
            }

            return res;
        }


        public static bool BrowseForDirectory(ref string path)
        {
            var ret = false;

            using (var dlg = new System.Windows.Forms.FolderBrowserDialog())
            {
                try
                {
                    // Try selecting an initial directory for user
                    if (!string.IsNullOrWhiteSpace(_lastSelectedDirectoryPath))
                    {
                        if (System.IO.Directory.Exists(_lastSelectedDirectoryPath))
                        {
                            dlg.SelectedPath = _lastSelectedDirectoryPath;
                        }
                        else
                        {
                            string dir = System.IO.Path.GetDirectoryName(_lastSelectedDirectoryPath);
                            if (!string.IsNullOrWhiteSpace(dir) && System.IO.Directory.Exists(dir))
                            {
                                dlg.SelectedPath = _lastSelectedDirectoryPath;
                            }
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(_lastSelectedFilePath))
                        {
                            dlg.SelectedPath = System.IO.Path.GetDirectoryName(_lastSelectedFilePath);
                        }
                    }
                }
                catch
                {
                    // Silently ignore exception, it won't affect normal operation
                }

                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    _lastSelectedDirectoryPath = dlg.SelectedPath;
                    path = dlg.SelectedPath;
                    ret = true;
                }
            }

            return ret;
        }


        public static bool BrowseForDirectory(ref string path, string initialPath)
        {
            if (string.IsNullOrWhiteSpace(initialPath)) return BrowseForDirectory(ref path);

            var ret = false;
            using (var dlg = new System.Windows.Forms.FolderBrowserDialog())
            {
                try
                {
                    // Try selecting an initial directory for user
                    if (!string.IsNullOrWhiteSpace(initialPath))
                    {
                        if (System.IO.Directory.Exists(initialPath))
                        {
                            dlg.SelectedPath = initialPath;
                            if (string.IsNullOrWhiteSpace(_lastSelectedDirectoryPath))
                            {
                                _lastSelectedDirectoryPath = initialPath;
                            }
                        }
                    }              
                }
                catch
                {
                    // Silently ignore exception, it won't affect normal operation
                }

                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    _lastSelectedDirectoryPath = dlg.SelectedPath;
                    path = dlg.SelectedPath;
                    ret = true;
                }
            }

            return ret;
        }
    }
}