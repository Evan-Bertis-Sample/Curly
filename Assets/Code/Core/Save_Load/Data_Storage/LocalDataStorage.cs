using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using CurlyCore.CurlyApp;

namespace CurlyCore.Saving
{
    [DataStorageMetadata("Local")]
    public class LocalDataStorage : IDataStorage
    {
        private string _SAVE_DIRECTORY => $"{Application.persistentDataPath}/savedata";

        [field: SerializeField]
        public string SaveExtension = "curly";

        public byte[] RetrieveData(string dataKey)
        {
            string path = CreatePath(dataKey);
            return File.ReadAllBytes(path);
        }

        public void StoreData(string dataKey, byte[] data)
        {
            if (Directory.Exists(_SAVE_DIRECTORY) == false)
            {
                App.Instance.Logger.Log(Debugging.LoggingGroupID.APP, $"Creating Save Directory: {_SAVE_DIRECTORY}");
                Directory.CreateDirectory(_SAVE_DIRECTORY);
            }

            string path = CreatePath(dataKey);

            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
            {
                fs.Write(data);
            }
        }

        public List<string> GetAllKeys()
        {
            List<string> filteredFiles = Directory
                .EnumerateFiles(_SAVE_DIRECTORY, "*", SearchOption.TopDirectoryOnly)
                .Where(file =>
                {
                    return Path.GetExtension(file) == SaveExtension;
                })
                .Select(path =>
                {
                    return Path.GetFileNameWithoutExtension(path);
                })
                .ToList();

            return filteredFiles;
        }

        private string CreatePath(string fileName)
        {
            return $"{_SAVE_DIRECTORY}/{fileName}.{SaveExtension}";
        }
    }
}