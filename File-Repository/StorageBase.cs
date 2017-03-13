using System;
using System.IO;

namespace File_Repository
{
    public abstract class StorageBase
    {
        public abstract string Save(string type, string Name, Stream file, string address);
        public abstract FileData Get(string key, string type);
    }
}
