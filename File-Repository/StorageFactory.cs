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
using System.Text;
using System.Linq;

namespace File_Repository
{
    public class StorageFactory
    {
        private string StorageType {get; set;}

        private List<StorageBase> StorageCollection = new List<StorageBase>();

        public StorageFactory()
        {
        }

        public StorageBase GetStorage(bool NewCopy, string storageType)
        {

            Type _type = null;
            StorageBase _storage = null;

            _type = Type.GetType(StorageType);

            if (NewCopy == true)
            {
                _storage = (StorageBase)Activator.CreateInstance(_type);
            }
            else
            {
                _storage = (from storageByType in StorageCollection where storageByType.GetType() == _type select storageByType).FirstOrDefault();

                if (_storage == null)
                {
                    _storage = (StorageBase)Activator.CreateInstance(_type);

                    StorageCollection.Add(_storage);
                }
            }
            
            return _storage;
        }

        public void ClearStorage()
        {
            StorageCollection.Clear();
        }
    }
}
