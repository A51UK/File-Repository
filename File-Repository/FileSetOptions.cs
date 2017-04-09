﻿/*
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

using File_Repository.Enum;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace File_Repository
{
    public class FileSetOptions
    {
        public string Folder { get; set; } = string.Empty;
        public string Key { get; set; } = string.Empty;
        public Stream _stream { get; set; } = null;
        public string ContentType { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public FileAccessLevel FileAccess { get; set; } = FileAccessLevel._private;
        public CloudSecureOptions CloudSecure { get; set; } = CloudSecureOptions.defualt;
        public string SecureFileLocation { get; set; } = string.Empty;
        public FolderOptions folderOptions { get; set; } = FolderOptions.CreateIfNull;
        public string ProjectId { get; set; } = string.Empty;
        public string ConfigurationString { get; set; } = string.Empty;
    }
}
