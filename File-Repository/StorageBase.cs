using System;
using System.IO;
using System.Threading.Tasks;

namespace File_Repository
{
    public abstract class StorageBase
    {
        public abstract Task<string> SaveAsync(FileSetOptions fileSetOptions);
        public abstract Task<FileData> GetAsync(FileGetOptions fileGetOptions);
    }
}
