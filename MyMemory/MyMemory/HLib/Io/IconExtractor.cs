

namespace HLib.Io
{

    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Windows;
    using System.Windows.Interop;
    using System.Windows.Media.Imaging;


    public class IconExtractor : IDisposable
    {

        private static IconExtractor instance;
        private static volatile object instanceLock = new object();
        private readonly Dictionary<string, BitmapSource> _cache;
        private readonly List<Icon> _icons;


        public IconExtractor()
        {
            this._icons = new List<Icon>();
            this._cache = new Dictionary<string, BitmapSource>();
        }


        public static IconExtractor Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new IconExtractor();
                    }
                }

                return instance;
            }
        }

        
        public void Dispose()
        {
            this._cache.Clear();

            foreach (var icon in this._icons)
            {
                icon.Dispose();
            }
            this._icons.Clear();
        }


        public BitmapSource ExtractByFilePath(string path)
        {
            // Invalid path
            if(!PathUtil.FilePathIsValid(path)) return null;

            // Unknown extension, use no image
            var fileExt = System.IO.Path.GetExtension(path);
            if (string.IsNullOrWhiteSpace(fileExt)) return null;

            // Found in chache
            if (this._cache.ContainsKey(fileExt)) return this._cache[fileExt];

            // Extract icon
            var icon = Icon.ExtractAssociatedIcon(path);
            if (icon == null) return null;
            this._icons.Add(icon);

            // Create bitmap source
            var image = Imaging.CreateBitmapSourceFromHIcon(
                icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

            this._cache.Add(fileExt, image);

            return image;
        }
    }
}
