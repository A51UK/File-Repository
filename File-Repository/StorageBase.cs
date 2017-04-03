using System;
using System.IO;

namespace File_Repository
{
    public abstract class StorageBase
    {
        public abstract string Save(FileSetOptions fileSetOptions);
        public abstract FileData Get(FileGetOptions fileGetOptions);
    }
}
