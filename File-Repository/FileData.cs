using System;
using System.Collections.Generic;
using System.Text;

namespace File_Repository
{
    public class FileData
    {
        public byte[] Stream { get; set; } = null;
        public string Type { get; set; } = string.Empty;
        public string Loc { get; set; } = string.Empty;
    }
}
