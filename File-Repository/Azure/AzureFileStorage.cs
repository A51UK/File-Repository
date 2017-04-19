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
using Microsoft.WindowsAzure.Storage.File;
using System.Threading.Tasks;

namespace File_Repository
{
    public class AzureFileStorage : StorageBase
    {
        public override async Task<FileData> GetAsync(FileGetOptions fileGetOptions)
        {
            FileData file = new FileData();

            CloudStorageAccount storageAccount = Authorized(fileGetOptions);

            CloudFileClient fileClient = storageAccount.CreateCloudFileClient();

            CloudFileShare fileshare = fileClient.GetShareReference(fileGetOptions.Folder);

            if (fileshare.ExistsAsync().Result)
            {
                CloudFileDirectory cFileDir = fileshare.GetRootDirectoryReference();

                CloudFile cFile = cFileDir.GetFileReference(fileGetOptions.Key);

                if(cFile.ExistsAsync().Result)
                {
                    await cFile.DownloadToStreamAsync(file.Stream);
                }
            }

            file.Type = "Azure File Storage";

            return file;
        }

        public override async Task<string> SaveAsync(FileSetOptions fileSetOptions)
        {

            FileData file = new FileData();

            CloudStorageAccount storageAccount = Authorized(fileSetOptions);

            CloudFileClient fileClient = storageAccount.CreateCloudFileClient();

            CloudFileShare fileshare = fileClient.GetShareReference(fileSetOptions.Folder);

            await fileshare.CreateIfNotExistsAsync();

            CloudFileDirectory cFileDir = fileshare.GetRootDirectoryReference();

            await cFileDir.CreateIfNotExistsAsync();

            CloudFile cFile = cFileDir.GetFileReference(fileSetOptions.Key);

            await cFile.UploadFromStreamAsync(fileSetOptions._stream);

            return fileSetOptions.Key;

        }

        private CloudStorageAccount Authorized(dynamic secureOptions)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(secureOptions.ConfigurationString);

            return storageAccount;
        }

    }
}
