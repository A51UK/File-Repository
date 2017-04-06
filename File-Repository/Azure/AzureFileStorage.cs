using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.WindowsAzure.Storage;
using System.Threading.Tasks;

namespace File_Repository
{
    public class AzureFileStorage : StorageBase
    {
        public override Task<FileData> GetAsync(FileGetOptions fileGetOptions)
        {
            throw new NotImplementedException();
        }

        public override Task<string> SaveAsync(FileSetOptions fileSetOptions)
        {
            throw new NotImplementedException();
        }
    }
}
