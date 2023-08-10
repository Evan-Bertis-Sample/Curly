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

            if (Directory.Exists(_SAVE_DIRECTORY) == false)
            {
                App.Instance.Logger.Log(Debugging.LoggingGroupID.APP, $"Creating Save Directory: {_SAVE_DIRECTORY}");
                Directory.CreateDirectory(_SAVE_DIRECTORY);
            }

            Save(null, new JsonSaveSerializer(), "test");
            string path = CreateSaveFilePath(SerializationType.TEXT, "test");
            Load(path);
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

        public SaveData Load(string savepath)
        {
            // Get serialization type
            SerializationType format = GetSerializationType(savepath);

            int byteCount = SaveHeader.GetByteSize(format);
            byte[] headerBuffer = new byte[byteCount];

            using (FileStream fs = new FileStream(savepath, FileMode.Open, FileAccess.Read))
            {
                int bytesRead = fs.Read(headerBuffer, 0, byteCount);
                fs.Close();

                if (bytesRead != headerBuffer.Length)
                {
                    throw new Exception("Header information is not expected length!");
                }
            }

            SaveHeader header = SaveHeader.Deserialize(headerBuffer, format);
            Debug.Log(header.ToString());
            ISaveDataSerializer serializer = _factory.CreateSerializer(header.SerializerID);

            return null;
        }

        private string CreateSaveFilePath(SerializationType type, string fileName = "")
        {
            if (fileName == "") fileName = Guid.NewGuid().ToString();

            string extension = _SAVE_EXTENSIONS[(int)type];

            return $"{_SAVE_DIRECTORY}/{fileName}{extension}";
        }

        private SerializationType GetSerializationType(string absolutePath)
        {
            string extension = Path.GetExtension(absolutePath);
            int index = Array.IndexOf(_SAVE_EXTENSIONS, extension);

            if (index == -1) throw new Exception("Unsupported Filetype!");

            return (SerializationType)index;
        }
    }
}
