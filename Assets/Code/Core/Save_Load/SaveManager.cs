using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using CurlyCore.CurlyApp;

namespace CurlyCore.Saving
{
    [CreateAssetMenu(menuName = "Curly/Core/Save Manager", fileName = "SaveManager")]
    public class SaveManager : BooterObject
    {
        private SerializerFactory _factory;
        private List<string> _saveFilePaths = new List<string>();

        private string _SAVE_DIRECTORY => $"{Application.persistentDataPath}/savedata";

        private readonly string[] _SAVE_EXTENSIONS = new string[]
        {
            ".curlybin", // BINARY
            ".curly" // TEXT
        };

        public override void OnBoot(App app, Scene scene)
        {
            _factory = new SerializerFactory();

            Debug.Log(_SAVE_DIRECTORY);

            if (Directory.Exists(_SAVE_DIRECTORY) == false)
            {
                App.Instance.Logger.Log(Debugging.LoggingGroupID.APP, $"Creating Save Directory: {_SAVE_DIRECTORY}");
                Directory.CreateDirectory(_SAVE_DIRECTORY);
            }

            Save(null, new JsonSaveSerializer());
        }

        public void Save(SaveData data, ISaveDataSerializer serializer, string fileName = "")
        {
            // Get the type of the serializer
            Type serializerType = serializer.GetType();
            // Get the SerializerMetadataAttribute from the type
            SerializerMetadataAttribute attribute = serializerType.GetCustomAttribute<SerializerMetadataAttribute>();

            // Check if the attribute is null (i.e., not present on the type)
            if (attribute == null)
            {
                throw new Exception("The provided serializer does not have the required SerializerMetadataAttribute!");
            }

            string path = CreateSaveFilePath(attribute.SerializationFormat, fileName);

            SaveHeader header = new SaveHeader(attribute.ID, attribute.SerializationFormat);
            Byte[] headerEncoding = header.Serialize();

            using (File.Create(path)) { }
            
            Byte[] dataEncoding = serializer.Save(data);

            Byte[] saveEncoding = headerEncoding.Concat(dataEncoding).ToArray();
            File.WriteAllBytes(path, saveEncoding);
        }

        private string CreateSaveFilePath(SerializationType type, string fileName = "")
        {
            if (fileName == "") fileName = Guid.NewGuid().ToString();

            string extension = _SAVE_EXTENSIONS[(int)type];

            return $"{_SAVE_DIRECTORY}/{fileName}{extension}";
        }
    }
}
