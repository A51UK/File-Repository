using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace File_Repository
{
    public class LocalStorageFileSystem : StorageBase
    {
        public override FileData Get(FileGetOptions fileGetOptions)
        {
            string fileAddress = Path.Combine(fileGetOptions.Address, fileGetOptions.Folder, fileGetOptions.Key);

            FileData file = new FileData();

            using (FileStream fs = new FileStream(fileAddress, FileMode.Open, FileAccess.Read))
            {
                fs.CopyTo(file.Stream);
            }

            file.Type = "Local File System";

            return file;
        }

        public override string Save(FileSetOptions fileSetOptions)
        {

            string fileAddress = Path.Combine(fileSetOptions.Address,fileSetOptions.Folder, fileSetOptions.Key);

            using (FileStream fs = new FileStream(fileAddress, FileMode.Create, FileAccess.Write))
            {
                fileSetOptions._stream.CopyTo(fileSetOptions._stream);
            }

            return fileSetOptions.Key;
        }
    }
}
