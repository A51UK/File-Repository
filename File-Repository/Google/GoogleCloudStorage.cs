/*
 *  Copyright 2017 Craig Lee Mark Adams

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
 * 
 * 
 * */

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Google.Cloud.Storage.V1;
using Google.Apis.Storage.v1;
using Google.Apis.Auth.OAuth2;
using File_Repository.Enum;
using System.Threading.Tasks;

namespace File_Repository
{
    public class GoogleCloudStorage : StorageBase
    {
        public override async Task<FileData> GetAsync(FileGetOptions fileGetOptions)
        {
            GoogleCredential credential = await AuthorizedAsync(fileGetOptions);
            StorageClient storageClient = await StorageClient.CreateAsync(credential);

            FileData file = new FileData();

            switch (fileGetOptions.FileTransfer)
            {
                case FileTransferOptions.Stream:
                    await storageClient.DownloadObjectAsync(fileGetOptions.Folder, fileGetOptions.Key, file.Stream);
                    break;

                case FileTransferOptions.Url:
                    var _file = await storageClient.GetObjectAsync(fileGetOptions.Folder, fileGetOptions.Key);

                    if (_file != null)
                    {
                        file.Loc = _file.MediaLink;
                    }
                    break;

                case FileTransferOptions.SecureUrl:
                    var urlSigner =  UrlSigner.FromServiceAccountCredential((ServiceAccountCredential)credential.UnderlyingCredential);

                    if (urlSigner != null)
                    {
                        urlSigner.Sign(fileGetOptions.Folder, fileGetOptions.Key,fileGetOptions.SecureLinkTimeToLive);
                    }
                    break;
            }

            file.Type = "Google Cloud Storage";

            return file;
        }

        public override async Task<string> SaveAsync(FileSetOptions fileSetOptions)
        {
            GoogleCredential credential = await AuthorizedAsync(fileSetOptions);

            StorageClient storageClinet = await StorageClient.CreateAsync(credential);

            PredefinedObjectAcl predefinedObjectAcl = PredefinedObjectAcl.ProjectPrivate;
            PredefinedBucketAcl predefinedBucketAcl = PredefinedBucketAcl.ProjectPrivate;

            switch (fileSetOptions.FileAccess)
            {
                case FileAccessLevel._private:
                    predefinedObjectAcl = PredefinedObjectAcl.AuthenticatedRead;
                    predefinedBucketAcl = PredefinedBucketAcl.AuthenticatedRead;
                    break;

                case FileAccessLevel._public:
                    predefinedObjectAcl = PredefinedObjectAcl.PublicRead;
                    predefinedBucketAcl = PredefinedBucketAcl.PublicRead;
                    break;
            }


            if (fileSetOptions.folderOptions == FolderOptions.CreateIfNull)
            {
                var folder = await storageClinet.GetBucketAsync(fileSetOptions.Folder);

                if (folder == null)
                {
                   await storageClinet.CreateBucketAsync(fileSetOptions.ProjectId, fileSetOptions.Folder, new CreateBucketOptions() { PredefinedAcl = predefinedBucketAcl, PredefinedDefaultObjectAcl = predefinedObjectAcl });
                }
            }

            fileSetOptions._stream.Position = 0;

            await storageClinet.UploadObjectAsync(fileSetOptions.Folder, fileSetOptions.Key, fileSetOptions.ContentType, fileSetOptions._stream, new UploadObjectOptions() { PredefinedAcl = predefinedObjectAcl });

            return fileSetOptions.Key;
        }

        private async Task<GoogleCredential> AuthorizedAsync(dynamic secureOptions)
        {
            switch (secureOptions.CloudSecure)
            {
                case CloudSecureOptions.defualt:
                    return await GoogleCredential.GetApplicationDefaultAsync();
                case CloudSecureOptions.file:
                    return await GoogleCredential.FromJson(secureOptions.SecureFileLocation);
                default:
                    return await GoogleCredential.GetApplicationDefaultAsync();
            }

        }
    }
}
