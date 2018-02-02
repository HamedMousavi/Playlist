using System;
using System.Security.Cryptography;
using System.Text;


namespace MyMemory.Domain
{

    public class FileResource : INameableResource
    {

        public FileResource(string path)
        {
            Path = path;

            using (var md5 = MD5.Create())
            {
                Id = BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(Path)));
            }
        }

        public string Name => string.IsNullOrWhiteSpace(Path) ? null : System.IO.Path.GetFileName(Path);
        public string Path { get; set; }
        public string Id { get; set; }
        public bool IsEqual(string id)
        {
            return string.Equals(Id, id, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}