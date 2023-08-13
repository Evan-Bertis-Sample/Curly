using System;

namespace CurlyCore
{
    public class DataStorageMetadata : Attribute
    {
        public string ID { get; private set; }

        public DataStorageMetadata(string id)
        {
            ID = id;
        }
    }
}
