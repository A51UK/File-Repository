using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace File_Repository
{
    public class AzureFileStorage : StorageBase
    {
        public override FileData Get(string key, string type)
        {
            throw new NotImplementedException();
        }

        public override string Save(string type, string name, Stream file, string address)
        {
            throw new NotImplementedException();
        }
    }
}
