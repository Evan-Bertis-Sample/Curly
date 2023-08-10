using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CurlyCore.Saving
{
    [SerializerMetadata("JSN", SerializationType.TEXT)]
    public class JsonSaveSerializer : ISaveDataSerializer
    {
        public SaveData Load(string absolutePath)
        {
            Debug.Log("loading");
            return null;
        }

        public byte[] Save(SaveData toSave)
        {
            string test = "Test String!";
            return System.Text.Encoding.UTF8.GetBytes(test);
        }
    }
}
