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
            throw new NotImplementedException();
        }

        public override string Save(string type, string name, Stream file, string address)
        {

            string fileAddress = Path.Combine(address, type, name);

            using (FileStream fs = new FileStream(fileAddress, FileMode.Create, FileAccess.Write))
            {
                file.CopyTo(file);
            }

            return name;
        }
    }
}
