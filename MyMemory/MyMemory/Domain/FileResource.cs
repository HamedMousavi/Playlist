using System;
using System.Security.Cryptography;
using System.Text;
using MyMemory.Domain.Abstract;


namespace MyMemory.Domain
{

    public class FileResource : IResource
    {

        public FileResource(string path)
        {
            Path = path;

            using (var md5 = MD5.Create())
            {
                Id = BitConverter.ToString(
                    md5
                    .ComputeHash(Encoding.UTF8.GetBytes(Path)))
                    .Replace("-", string.Empty);
            }
        }

        public string Name => string.IsNullOrWhiteSpace(Path) ? null : System.IO.Path.GetFileName(Path);
        public string Path { get; set; }
        public string Id { get; set; }
    }
}