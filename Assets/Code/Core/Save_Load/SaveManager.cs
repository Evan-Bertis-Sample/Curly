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
        [field: SerializeField] public List<string> SaveFilePaths { get; private set; } = new List<string>();
        public SaveData CurrentSave { get; private set; }
        private SerializerFactory _factory;

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
            SaveFilePaths = GetAllSavePaths();

            SaveData data = new SaveData();
            data.Save<int>("TestFact", 3);
            Save(data, new JsonSaveSerializer(), "test");

            string path = CreateSaveFilePath(SerializationType.TEXT, "test");
            SaveData load = Load(path);
            int testLoad = -1;
            load.Load("TestFact", ref testLoad, -1);
            Debug.Log("Loaded: " + testLoad);
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

            if (File.Exists(path) == false) using (File.Create(path)) { }

            Byte[] dataEncoding = serializer.Save(data);

            Byte[] saveEncoding = headerEncoding.Concat(dataEncoding).ToArray();
            File.WriteAllBytes(path, saveEncoding);
        }

        public SaveData Load(string savepath)
        {
            // Get serialization type
            SerializationType format = GetSerializationType(savepath);

            using (FileStream fs = new FileStream(savepath, FileMode.Open, FileAccess.Read))
            {
                int headerSize = SaveHeader.GetByteSize(format);
                byte[] headerBuffer = new byte[headerSize];
                int bytesRead = fs.Read(headerBuffer, 0, headerSize);

                if (bytesRead != headerBuffer.Length)
                {
                    throw new Exception("Header information is not expected length!");
                }

                SaveHeader header = SaveHeader.Deserialize(headerBuffer, format);
                ISaveDataSerializer serializer = _factory.CreateSerializer(header.SerializerID);

                int dataSize = (int)fs.Length - headerSize;
                Debug.Log(dataSize);
                byte[] dataBuffer = new byte[dataSize];
                fs.Read(dataBuffer, 0, dataSize);
                SaveData data = serializer.Load(dataBuffer);
                fs.Close();

                return data;
            }
        }

        private List<string> GetAllSavePaths()
        {
            List<string> filteredFiles = Directory
                .EnumerateFiles(_SAVE_DIRECTORY, "*", SearchOption.TopDirectoryOnly)
                .Where(file =>
                {
                    // Debug.Log(File);
                    return _SAVE_EXTENSIONS.Contains(Path.GetExtension(file));
                })
                .Select(path => path.Replace("\\", "/"))
                .ToList();

            return filteredFiles;
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
