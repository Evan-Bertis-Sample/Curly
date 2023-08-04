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
        public class RefrenceCache : SerializableDictionary<AssetReference, AudioOverrideGroup> {}

        [field: SerializeField, DirectoryPath] public string AudioDirectoryRoot { get; private set; }
        [field: SerializeField] public string ReplacementPath { get; private set; } = "Audio";
        [field: SerializeField] public string OverrideGroupAddressName { get; private set; } = "Override.asset";
        [field: SerializeField] public AudioOverrideGroup DefaultGroupSettings { get; private set; }
        [field: SerializeField] public RefrenceCache GroupCache {get; private set;}

        public readonly string AUDIO_GROUP_NAME = "Audio";
        public readonly string CLIP_LABEL = "Clips";
        public readonly string OVERRIDE_LABEL = "Override";

        private Dictionary<AssetReference, AudioOverrideGroup> _groupByReference = new Dictionary<AssetReference, AudioOverrideGroup>();

        // public override async Task OnBootAsync(App app, Scene scene)
        // {
        //     // bootin bootin bootin
        //     await CreateCacheAsync();
        // }

        // private async Task CreateCacheAsync()
        // {
        //     // Prepare our mapping dictionary
        //     Dictionary<string, AudioOverrideGroup> pathsToGroup = new Dictionary<string, AudioOverrideGroup>();
        //     List<string> directories = Directory.GetDirectories(AudioDirectoryRoot, "*", SearchOption.AllDirectories).ToList();
        //     directories = directories.Select(x => x.Replace("\\", "/")).ToList();
        //     App.Instance.Logger.Log(LoggingGroupID.APP, System.String.Join(", ", directories));
        //     // Fill with null data
        //     foreach (string dir in directories) pathsToGroup[dir] = null;

        //     // Now load all the overrides
        //     var locations = await Addressables.LoadResourceLocationsAsync(OVERRIDE_LABEL, null).Task;

        //     if (locations == null) return;

        //     foreach(IResourceLocation location in locations)
        //     {
        //         AssetReference reference = await Addressables.LoadAssetAsync<AssetReference>(location).Task;
        //     }
        // }

        public void SetCache(Dictionary<AssetReference, AudioOverrideGroup> cache)
        {
            GroupCache = new RefrenceCache();
            foreach (var pair in cache)
            {
                GroupCache[pair.Key] = pair.Value;
            }
        }
    }
}