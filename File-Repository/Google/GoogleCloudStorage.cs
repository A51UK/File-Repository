﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Google.Cloud.Storage.V1;
using Google.Apis.Storage.v1;
using Google.Apis.Auth.OAuth2;
using File_Repository.Enum;

namespace File_Repository
{
    public class GoogleCloudStorage : StorageBase
    {
        public override FileData Get(FileGetOptions fileGetOptions)
        {
            GoogleCredential credential = Authorized(fileGetOptions);
            StorageClient storageClient = StorageClient.Create(credential);

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

                case FileTransferOptions.SecureUrl:
                    var urlSigner = UrlSigner.FromServiceAccountCredential((ServiceAccountCredential)credential.UnderlyingCredential);

                    if (urlSigner != null)
                    {
                        urlSigner.Sign(fileGetOptions.Folder, fileGetOptions.Key, );
                    }
                    break;
            }

            file.Type = "Google Cloud Storage";

            return file;
        }

        public override string Save(FileSetOptions fileSetOptions)
        {
            GoogleCredential credential = Authorized(fileSetOptions);

            StorageClient storageClinet = StorageClient.Create(credential);

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

        private GoogleCredential Authorized(dynamic secureOptions)
        {
            switch (secureOptions.CloudSecure)
            {
                case CloudSecureOptions.defualt:
                    return GoogleCredential.GetApplicationDefaultAsync().Result;
                case CloudSecureOptions.file:
                    return GoogleCredential.FromJson(secureOptions.SecureFileLocation);
                default:
                    return GoogleCredential.GetApplicationDefaultAsync().Result;
            }

        }
    }
}
