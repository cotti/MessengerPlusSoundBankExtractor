using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessengerPlusSoundBankExtractor.Models
{
    public class AudioFile
    {
        private string? name;

        private ReadOnlyMemory<byte> file;
        public string Name { get => name!; set => name = value; }

        public ReadOnlyMemory<byte> File { get => file; set => file = value; }

        public AudioFile(string name, ReadOnlyMemory<byte> file)
        {
            this.name = name;
            this.file = file;
        }

        public AudioFile() { }
    }
}
