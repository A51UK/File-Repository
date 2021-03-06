﻿/*
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
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System.IO;
using File_Repository;
using System.Threading.Tasks;
using System.Net.Http;

namespace File_Repository_Tests
{
    [TestClass]
    public class AzureFileStorageTests
    {

        private ConfigurationRoot configuration = null;

        [TestInitialize]
        public void Initialize()
        {
            var builder = new ConfigurationBuilder().SetBasePath(AppContext.BaseDirectory).AddJsonFile("appsetting.json");

            configuration = (ConfigurationRoot)builder.Build();
        }

        [TestMethod]
        public void SaveFileTest()
        {

        }


        [TestMethod]
        public void LoadFileTest()
        {

        }
    }
}
