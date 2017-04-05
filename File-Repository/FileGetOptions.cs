using File_Repository.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace File_Repository
{
    public class FileGetOptions
    {
        public string Folder { get; set; } = string.Empty;
        public string Key { get; set; } = string.Empty;
        public FileTransferOptions FileTransfer { get; set; } = FileTransferOptions.Stream;
        public string Address { get; set; } = string.Empty;
        public CloudSecureOptions CloudSecure { get; set; } = CloudSecureOptions.defualt;
        public string SecureFileLocation { get; set; } = string.Empty;
        public TimeSpan SecureLinkTimeToLive { get; set; } = new TimeSpan(1, 0, 0);

    }
}
