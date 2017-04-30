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
using System.Threading.Tasks;

namespace File_Repository
{
    public class LocalStorageFileSystem : StorageBase
    {
        public async override Task<FileData> GetAsync(FileGetOptions fileGetOptions)
        {
            string fileAddress = Path.Combine(fileGetOptions.Address, fileGetOptions.Folder, fileGetOptions.Key);

            FileData file = new FileData();

            using (FileStream fs = new FileStream(fileAddress, FileMode.Open, FileAccess.Read))
            {

                await fs.CopyToAsync(file.Stream,(int)fs.Length);
            }

            file.Type = "Local File System";

            return file;
        }

        public async override Task<string> SaveAsync(FileSetOptions fileSetOptions)
        {

            string subAddress = Path.Combine(fileSetOptions.Address, fileSetOptions.Folder);
            string fileAddress = Path.Combine(subAddress, fileSetOptions.Key);

            if(fileSetOptions.folderOptions == Enum.FolderOptions.CreateIfNull)
            {
                if(Directory.Exists(subAddress) == false)
                {
                    Directory.CreateDirectory(subAddress);
                }
            }

            using (FileStream fs = new FileStream(fileAddress, FileMode.Create, FileAccess.Write))
            {
                fileSetOptions._stream.Position = 0;
 
                await fileSetOptions._stream.CopyToAsync(fs);
            }

            return fileSetOptions.Key;
        }
    }
}
