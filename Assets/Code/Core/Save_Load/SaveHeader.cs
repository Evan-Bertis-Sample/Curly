using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Newtonsoft.Json;

namespace CurlyCore.Saving
{
    [System.Serializable]
    public struct SaveHeader
    {
        [field: SerializeField] public string SerializerID { get; } // Must be 3 characters -- 3 bytes
        [field: SerializeField] public SerializationType SerializationType { get; } // 4 bytes
        [field: SerializeField] public DateTime TimeSaved { get; } // 8 bytes

        public SaveHeader(string id, SerializationType type)
        {
            if (id.Length != 3) throw new Exception($"Cannot make SaveHeader: Serialization ID must be 3 characters! Please change ID '{id}' to be 3 characters!");
            SerializerID = id;
            SerializationType = type;
            TimeSaved = DateTime.Now;
        }

        public Byte[] Serialize()
        {
            switch (SerializationType)
            {
                case SerializationType.TEXT:
                    return SerializeAsText();
                case SerializationType.BINARY:
                    return SerializeAsBinary();
                default:
                    throw new Exception("Invalid SerializationType!");
            }
        }

        private Byte[] SerializeAsText()
        {
            string textRepresentation = JsonConvert.SerializeObject(this);
            return Encoding.UTF8.GetBytes(textRepresentation);
        }

        private Byte[] SerializeAsBinary()
        {
            Byte[] result = new Byte[15]; // 3 bytes for SerializerID, 4 bytes for SerializationType, 8 bytes for TimeSaved

            // Serialize SerializerID (3 bytes)
            Buffer.BlockCopy(Encoding.ASCII.GetBytes(SerializerID), 0, result, 0, 3);

            // Serialize SerializationType (4 bytes)
            Buffer.BlockCopy(BitConverter.GetBytes((int)SerializationType), 0, result, 3, 4);

            // Serialize TimeSaved (8 bytes)
            Buffer.BlockCopy(BitConverter.GetBytes(TimeSaved.ToBinary()), 0, result, 7, 8);

            return result;
        }

        public static int GetByteSize(SerializationType type)
        {
            switch (type)
            {
                case SerializationType.BINARY:
                    return 15;
                case SerializationType.TEXT:
                    return GetTextSerializationByteSize();
                default:
                    throw new System.Exception("Invalid type!");
            }
        }

        // Just a quick way to get the length of metadata
        private static int GetTextSerializationByteSize()
        {
            SaveHeader fake = new SaveHeader("FKE", SerializationType.TEXT);
            string json = JsonConvert.SerializeObject(fake);
            // Debug.Log(json);
            return System.Text.ASCIIEncoding.UTF8.GetByteCount(json);
        }
    }
}
