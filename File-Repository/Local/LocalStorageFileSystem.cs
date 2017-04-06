using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace File_Repository
{
    public class LocalStorageFileSystem : StorageBase
    {
        public async override Task<FileData> GetAsync(FileGetOptions fileGetOptions)
        {
            string fileAddress = Path.Combine(fileGetOptions.Address, fileGetOptions.Folder, fileGetOptions.Key);

            FileData file = new FileData();

            using (FileStream fs = new FileStream(fileAddress, FileMode.Open, FileAccess.Read))
            {
                await fs.CopyToAsync(file.Stream);
            }

            file.Type = "Local File System";

            return file;
        }

        public async override Task<string> SaveAsync(FileSetOptions fileSetOptions)
        {

            string subAddress = Path.Combine(fileSetOptions.Address, fileSetOptions.Folder);
            string fileAddress = Path.Combine(subAddress, fileSetOptions.Key);

            if(fileSetOptions.folderOptions == Enum.FolderOptions.CreateIfNull)
            {
                if(Directory.Exists(subAddress) == false)
                {
                    Directory.CreateDirectory(subAddress);
                }
            }

            using (FileStream fs = new FileStream(fileAddress, FileMode.Create, FileAccess.Write))
            {
                await fileSetOptions._stream.CopyToAsync(fileSetOptions._stream);
            }

            return fileSetOptions.Key;
        }
    }
}
