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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using File_Repository;

namespace File_Repository_Tests
{
    [TestClass]
    public class StorageFactoryTests
    {
        [TestMethod]
        public void MakeAzureBlobTest()
        {
            var storageFactory = new StorageFactory();

            var azureBlob = storageFactory.GetStorage(true, "AzureBlob");

            Assert.AreEqual(typeof(AzureBlob), azureBlob.GetType());

        }

        [TestMethod]
        public void MakeAzureFileStorageTest()
        {
            var storageFactory = new StorageFactory();

            var azureBlob = storageFactory.GetStorage(true, "AzureFileStorage");

            Assert.AreEqual(typeof(AzureFileStorage), azureBlob.GetType());
        }


        [TestMethod]
        public void MakeGoogleCloudStorageTest()
        {
            var storageFactory = new StorageFactory();

            var azureBlob = storageFactory.GetStorage(true, "GoogleCloudStorage");

            Assert.AreEqual(typeof(GoogleCloudStorage), azureBlob.GetType());
        }

        [TestMethod]
        public void MakeLocalStorageFileSystemTest()
        {
            var storageFactory = new StorageFactory();

            var azureBlob = storageFactory.GetStorage(true, "LocalStorageFileSystem");

            Assert.AreEqual(typeof(LocalStorageFileSystem), azureBlob.GetType());
        }
    }
}
