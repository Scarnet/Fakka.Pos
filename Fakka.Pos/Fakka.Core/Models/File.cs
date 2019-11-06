using System.Net.Http;

namespace Fakka.Core.Models
{
    public class File
    {
        public File(string name, string filepath, StreamContent content)
        {
            Name = name;
            FilePath = filepath;
            Content= content;
        }
        public string Name { get; set; }
        public string FilePath { get; set; }
        public StreamContent Content { get; set; }

    }
}