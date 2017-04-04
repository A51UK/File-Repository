using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace File_Repository
{
    public class FileSetOptions
    {
        public string Folder { get; set; } = string.Empty;
        public string Key { get; set; } = string.Empty;
        public Stream _stream { get; set; } = null;
        public string ContentType { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public FileAccessLevel FileAccess { get; set; } = FileAccessLevel._private;
        public CloudSecureOptions CloudSecure { get; set; } = CloudSecureOptions.defualt;
        public string SecureFileLocation { get; set; } = string.Empty;
    }
}
