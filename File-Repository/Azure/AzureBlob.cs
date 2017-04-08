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

            CloudStorageAccount storageAccount = Authorized(fileGetOptions.ConfigurationString);

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

            CloudStorageAccount storageAccount = Authorized(fileSetOptions.ConfigurationString);

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
