using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Google.Cloud.Storage.V1;

namespace File_Repository
{
    public class GoogleCloudStorage : StorageBase
    {
        public override FileData Get(FileGetOptions fileGetOptions)
        {

            StorageClient storageClient = StorageClient.Create();

            FileData file = new FileData();

            storageClient.DownloadObject(fileGetOptions.Folder, fileGetOptions.Key, file.Stream);

            file.Type = "Google Cloud Storage";

            return file;
        }

        public override string Save(string type, string name, Stream file, string address)
        {
            throw new NotImplementedException();
        }
    }
}
