/*
 *  Copyright 2017 Criag Lee Mark Adams

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
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Threading.Tasks;

namespace File_Repository
{
    public class AzureBlob : StorageBase
    {
        public override async Task<FileData> GetAsync(FileGetOptions fileGetOptions)
        {
            FileData file = new FileData();

            CloudStorageAccount storageAccount = Authorized(fileGetOptions);

            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            CloudBlobContainer container = blobClient.GetContainerReference(fileGetOptions.Folder);

            CloudBlockBlob blockBlob = container.GetBlockBlobReference(fileGetOptions.Key);

            switch (fileGetOptions.FileTransfer)
            {
                case Enum.FileTransferOptions.Stream:
                    await blockBlob.DownloadToStreamAsync(file.Stream);
                    break;

                case Enum.FileTransferOptions.Url:
                    file.Loc = blockBlob.Uri.AbsoluteUri;
                    break;

                case Enum.FileTransferOptions.SecureUrl:

                    var policy = new SharedAccessBlobPolicy()
                    {
                        Permissions = SharedAccessBlobPermissions.Read,
                        SharedAccessExpiryTime = DateTime.UtcNow + fileGetOptions.SecureLinkTimeToLive
                    };

                    file.Loc = (blockBlob.Uri.AbsoluteUri + blockBlob.GetSharedAccessSignature(policy));
                    break;
            }

            file.Type = "Mircosoft Azure Blob Storage";

            return file;
        }

        public override async Task<string> SaveAsync(FileSetOptions fileSetOptions)
        {

            CloudStorageAccount storageAccount = Authorized(fileSetOptions);

            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            CloudBlobContainer container = blobClient.GetContainerReference(fileSetOptions.Folder);

            if (await container.CreateIfNotExistsAsync())
            {
                if (fileSetOptions.FileAccess == FileAccessLevel._public)
                {
                    var publicAccess = BlobContainerPublicAccessType.Blob;
                    var sharedAccessPolicies = new SharedAccessBlobPolicy
                    {
                        Permissions = SharedAccessBlobPermissions.Read,
                    };

                    await container.SetPermissionsAsync(new BlobContainerPermissions() { PublicAccess = publicAccess });
                }
            }

            CloudBlockBlob blockBlob = container.GetBlockBlobReference(fileSetOptions.Key);

            await blockBlob.UploadFromStreamAsync(fileSetOptions._stream);

            return fileSetOptions.Key;
        }

        private CloudStorageAccount Authorized(dynamic secureOptions)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(secureOptions.ConfigurationString);

            return storageAccount;
        }
    }
}
