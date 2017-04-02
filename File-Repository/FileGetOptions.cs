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
    }

    public enum FileTransferOptions
    {
        Url,
        Stream
    }
}
