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
                    file.Loc = await blockBlob.AcquireLeaseAsync(null);
                    break;

                case Enum.FileTransferOptions.SecureUrl:
                    file.Loc = await blockBlob.AcquireLeaseAsync(fileGetOptions.SecureLinkTimeToLive);
                    break;
            }

            file.Type = "Mircosoft Azure Blob Storage";

            return file;
        }

        public override async Task<string> SaveAsync(FileSetOptions fileSetOptions)
        {
            throw new NotImplementedException();
        }

        private CloudStorageAccount Authorized(dynamic secureOptions)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(secureOptions.ConfigurationString);

            return storageAccount;
        }
    }
}
