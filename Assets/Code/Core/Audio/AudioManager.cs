using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using System.Linq;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.Rendering;

using CurlyCore.CurlyApp;
using CurlyUtility;
using CurlyCore.Debugging;

namespace CurlyCore.Audio
{
    [CreateAssetMenu(menuName = "Curly/Core/Audio Manager", fileName = "AudioManager")]
    public class AudioManager : BooterObject
    {
        [System.Serializable]
        public class SerializableOverrideCache : SerializableDictionary<string, AudioOverride> {}

        [System.Serializable]
        public class SerializableGroupCache : SerializableDictionary<string, AudioGroup> {}

        [field: SerializeField, DirectoryPath] public string AudioDirectoryRoot { get; private set; }
        [field: SerializeField] public string ReplacementPath { get; private set; } = "Audio";
        [field: SerializeField] public string OverrideGroupAddressName { get; private set; } = "Override.asset";
        [field: SerializeField] public AudioOverride DefaultGroupSettings { get; private set; }
        [field: SerializeField] public SerializableOverrideCache OverrideCache {get; private set;}
        [field: SerializeField] public SerializableGroupCache GroupCache {get; private set;}

        public readonly string AUDIO_GROUP_NAME = "Audio";
        public readonly string CLIP_LABEL = "Clips";
        public readonly string OVERRIDE_LABEL = "Override";


        public void SetOverrideCache(Dictionary<string, AudioOverride> cache)
        {
            OverrideCache = new SerializableOverrideCache();
            foreach (var pair in cache)
            {
                OverrideCache[pair.Key] = pair.Value;
            }
        }

        public void SetGroupCache(Dictionary<string, AudioGroup> cache)
        {
            GroupCache = new SerializableGroupCache();
            foreach(var pair in cache)
            {
                GroupCache[pair.Key] = pair.Value;
            }
        }
    }
}