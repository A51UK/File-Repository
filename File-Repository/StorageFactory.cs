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
