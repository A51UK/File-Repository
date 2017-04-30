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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using File_Repository;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Threading.Tasks;

namespace File_Repository_Tests
{
    [TestClass]
    public class LocalStorageFileSystemTests
    {
   
        private ConfigurationRoot configuration = null;

        [TestInitialize]
        public void Initialize()
        {
            var builder = new ConfigurationBuilder().SetBasePath(AppContext.BaseDirectory).AddJsonFile("appsetting.json");

            configuration = (ConfigurationRoot)builder.Build();
        }

        [TestMethod]
        public async Task SaveFileTestAsync()
        {
            MemoryStream stream = new MemoryStream();

            string fileLoc = Path.Combine(configuration["TestFolderAddress"], "LoadFile", "TestFile.txt");

            using (var file = File.OpenRead(fileLoc))
            {
                file.Position = 0;
                await file.CopyToAsync(stream);
            }

            LocalStorageFileSystem localStorageFileSystem = new LocalStorageFileSystem();

            FileSetOptions fileSetOptions = new FileSetOptions() { Address = configuration["TestFolderAddress"], Folder = "SaveFile", Key = "TestFile.txt", _stream = stream };

            var fileName = await localStorageFileSystem.SaveAsync(fileSetOptions);

            string saveFileLoc = Path.Combine(configuration["TestFolderAddress"], "SaveFile", "TestFile.txt");

            bool fileExists = File.Exists(saveFileLoc);

            File.Delete(saveFileLoc);

            Assert.IsTrue(fileExists);
        }

        [TestMethod]
        public async Task LoadFileTestAsync()
        {
            LocalStorageFileSystem localStorageFileSystem = new LocalStorageFileSystem();

            FileGetOptions fileGetOptions = new FileGetOptions() { Address = configuration["TestFolderAddress"], Folder = "LoadFile", Key = "TestFile.txt" };

            var fileInformation = await localStorageFileSystem.GetAsync(fileGetOptions);

            Assert.IsNotNull(fileInformation.Stream);

        }
    }
}
