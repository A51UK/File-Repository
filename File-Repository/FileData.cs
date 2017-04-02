using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
namespace File_Repository
{
    public class FileData
    {
        public MemoryStream Stream { get; set; } = null;
        public string Type { get; set; } = string.Empty;
        public string Loc { get; set; } = string.Empty;
    }
}
