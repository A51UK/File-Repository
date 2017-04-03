using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.WindowsAzure.Storage;

namespace File_Repository
{
    public class AzureFileStorage : StorageBase
    {
        public override FileData Get(FileGetOptions fileGetOptions)
        {
            throw new NotImplementedException();
        }

        public override string Save(FileSetOptions fileSetOptions)
        {
            throw new NotImplementedException();
        }
    }
}
