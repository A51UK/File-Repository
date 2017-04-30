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


using File_Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using File_Repository.Enum;

namespace File_Repository_Tests
{
    [TestClass]
    public class AzureBlobTests
    {
        private ConfigurationRoot configuration = null;

        [TestInitialize]
        public void Initialize()
        {
            var builder = new ConfigurationBuilder().SetBasePath(AppContext.BaseDirectory).AddJsonFile("appsetting.json");

            configuration = (ConfigurationRoot)builder.Build();
        }

        [TestMethod]
        public async Task SaveFilePublicTest()
        {
            MemoryStream stream = new MemoryStream();

            string fileLoc = Path.Combine(configuration["TestFolderAddress"], "LoadFile", "TestFile.txt");

            using (var file = File.OpenRead(fileLoc))
            {
                file.Position = 0;
                await file.CopyToAsync(stream);
            }


            var azureBlob = new AzureBlob();

            FileSetOptions fileSetOptions = new FileSetOptions() { FileAccess = FileAccessLevel._public, ConfigurationString = configuration["AzureStorageConnectionString"], Folder = "SaveFilePublic", Key = "TestFile.txt", _stream = stream };

            var fileName = await azureBlob.SaveAsync(fileSetOptions);

            HttpClient clinet = new HttpClient();
            var rfile = await clinet.GetStringAsync("http://127.0.0.1:10000/devstoreaccount1/savefile/TestFile.txt");

            Assert.IsTrue(rfile.Length > 0);

        }


        [TestMethod]
        public async Task SaveFilePrivateTest()
        {
            MemoryStream stream = new MemoryStream();

            string fileLoc = Path.Combine(configuration["TestFolderAddress"], "LoadFile", "TestFile.txt");

            using (var file = File.OpenRead(fileLoc))
            {
                file.Position = 0;
                await file.CopyToAsync(stream);
            }

            var azureBlob = new AzureBlob();

            FileSetOptions fileSetOptions = new FileSetOptions() { FileAccess = FileAccessLevel._private, ConfigurationString = configuration["AzureStorageConnectionString"], Folder = "SaveFilePrivate", Key = "TestFile.txt", _stream = stream };

            var fileName = await azureBlob.SaveAsync(fileSetOptions);

            Assert.IsTrue(fileName != string.Empty);

        }

        [TestMethod]
        public async Task LoadFileTestURLAsync()
        {
            MemoryStream stream = new MemoryStream();

            string fileLoc = Path.Combine(configuration["TestFolderAddress"], "LoadFile", "TestFile.txt");

            using (var file = File.OpenRead(fileLoc))
            {
                file.Position = 0;
                await file.CopyToAsync(stream);
            }

            var azureBlob = new AzureBlob();

            FileSetOptions fileSetOptions = new FileSetOptions() { FileAccess = FileAccessLevel._public, ConfigurationString = configuration["AzureStorageConnectionString"], Folder = "LoadFilePublic", Key = "TestFile.txt", _stream = stream };

            var fileName = await azureBlob.SaveAsync(fileSetOptions);

            FileGetOptions fileGetOptions = new FileGetOptions() { ConfigurationString = configuration["AzureStorageConnectionString"], Folder = "LoadFilePublic", Key = "TestFile.txt", FileTransfer = FileTransferOptions.Url };

            FileData data = await azureBlob.GetAsync(fileGetOptions);

            HttpClient webClient = new HttpClient();

            var fileData = await webClient.GetStringAsync(data.Loc);

            Assert.IsTrue(fileData.Length > 0);
        }

        [TestMethod]
        public async Task LoadFileTestStreamAsync()
        {
            MemoryStream stream = new MemoryStream();

            string fileLoc = Path.Combine(configuration["TestFolderAddress"], "LoadFile", "TestFile.txt");

            using (var file = File.OpenRead(fileLoc))
            {
                file.Position = 0;
                await file.CopyToAsync(stream);
            }

            var azureBlob = new AzureBlob();

            FileSetOptions fileSetOptions = new FileSetOptions() { FileAccess = FileAccessLevel._public, ConfigurationString = configuration["AzureStorageConnectionString"], Folder = "LoadFilePublic", Key = "TestFile.txt", _stream = stream };

            var fileName = await azureBlob.SaveAsync(fileSetOptions);

            FileGetOptions fileGetOptions = new FileGetOptions() { ConfigurationString = configuration["AzureStorageConnectionString"], Folder = "LoadFilePublic", Key = "TestFile.txt", FileTransfer = FileTransferOptions.Stream };

            FileData data = await azureBlob.GetAsync(fileGetOptions);

            HttpClient webClient = new HttpClient();

            Assert.IsTrue(data.Stream.Length > 0);
        }


        [TestMethod]
        public async Task LoadFileTestSecureURLAsync()
        {
            MemoryStream stream = new MemoryStream();

            string fileLoc = Path.Combine(configuration["TestFolderAddress"], "LoadFile", "TestFile.txt");

            using (var file = File.OpenRead(fileLoc))
            {
                file.Position = 0;
                await file.CopyToAsync(stream);
            }

            var azureBlob = new AzureBlob();

            FileSetOptions fileSetOptions = new FileSetOptions() { FileAccess = FileAccessLevel._public, ConfigurationString = configuration["AzureStorageConnectionString"], Folder = "LoadFilePrivate", Key = "TestFile.txt", _stream = stream };

            var fileName = await azureBlob.SaveAsync(fileSetOptions);

            FileGetOptions fileGetOptions = new FileGetOptions() { ConfigurationString = configuration["AzureStorageConnectionString"], Folder = "LoadFilePrivate", Key = "TestFile.txt", FileTransfer = FileTransferOptions.SecureUrl, SecureLinkTimeToLive = new TimeSpan(0,0,5) };

            FileData data = await azureBlob.GetAsync(fileGetOptions);

            HttpClient webClient = new HttpClient();

            var fileData = await webClient.GetStringAsync(data.Loc);

            Assert.IsTrue(fileData.Length > 0);
        }

    }
}
