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

            switch (fileGetOptions.FileTransfer)
            {
                case FileTransferOptions.Stream:
                    storageClient.DownloadObject(fileGetOptions.Folder, fileGetOptions.Key, file.Stream);
                    break;
                case FileTransferOptions.Url:
                    var _file = storageClient.GetObject(fileGetOptions.Folder, fileGetOptions.Key);
                    if (_file != null)
                    {
                        file.Loc = _file.MediaLink;
                    }
                    break;
            }

            file.Type = "Google Cloud Storage";

            return file;
        }

        public override string Save(FileSetOptions fileSetOptions)
        {

            StorageClient storageClinet = StorageClient.Create();

            PredefinedObjectAcl predefinedObjectAcl = PredefinedObjectAcl.ProjectPrivate; 

            switch (fileSetOptions.FileAccess)
            {
                case FileAccessLevel._private:
                    predefinedObjectAcl = PredefinedObjectAcl.AuthenticatedRead;
                    break;
                case FileAccessLevel._public:
                    predefinedObjectAcl = PredefinedObjectAcl.PublicRead;
                    break;
            }

            storageClinet.UploadObject(fileSetOptions.Folder, fileSetOptions.Key, fileSetOptions.ContentType, fileSetOptions._stream, new UploadObjectOptions() { PredefinedAcl = predefinedObjectAcl });

            return fileSetOptions.Key;
        }
    }
}
