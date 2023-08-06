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
using CurlyCore.Debugging;
using CurlyUtility;

namespace CurlyCore.Audio
{
    [CreateAssetMenu(menuName = "Curly/Core/Audio Manager", fileName = "AudioManager")]
    public class AudioManager : BooterObject
    {
        [System.Serializable]
        public class SerializableOverrideCache : SerializableDictionary<string, AudioOverride> { }

        [System.Serializable]
        public class SerializableGroupCache : SerializableDictionary<string, AudioGroup> { }

        [field: SerializeField, DirectoryPath] public string AudioDirectoryRoot { get; private set; }
        [field: SerializeField] public string ReplacementPath { get; private set; } = "Audio";
        [field: SerializeField] public string OverrideGroupAddressName { get; private set; } = "Override.asset";
        [field: SerializeField] public AudioOverride DefaultGroupSettings { get; private set; }
        [field: SerializeField] public SerializableOverrideCache OverrideCache { get; private set; }
        [field: SerializeField] public SerializableGroupCache GroupCache { get; private set; }

        [field: SerializeField] private readonly int _startingSourceCount = 10;
        [field: SerializeField] private readonly float _sourceGrowthRate = 1.5f;

        private GameObject _sourceParent;
        private List<AudioSource> _availableSources = new List<AudioSource>();
        private List<AudioSource> _unavailableSources = new List<AudioSource>();

        public readonly string AUDIO_GROUP_NAME = "Audio";
        public readonly string CLIP_LABEL = "Clips";
        public readonly string OVERRIDE_LABEL = "Override";
        public readonly string SOURCE_PARENT_NAME = "Audio Sources";
        public readonly string SOURCE_OBJECT_NAME = "Audio Source";

        public override void OnBoot(App app, Scene startingScene)
        {
            // Create audio sources
            _sourceParent = new GameObject(SOURCE_PARENT_NAME);
            GenerateSources(_sourceParent, _startingSourceCount);
            DontDestroyOnLoad(_sourceParent);
        }

        public AudioCallback PlayOneShot(string soundPath, Vector3 position = default, IAudioOverride iOverride = null)
        {
            bool found = GroupCache.TryGetValue(soundPath, out AudioGroup group);

            if (found == false)
            {
                App.Instance.Logger.Log(LoggingGroupID.APP, $"Could not find Audio Group for path {soundPath}", LogType.Warning);
                return null;
            }

            AudioOverride groupOverride = group.Override;
            IAudioOverride appliedOverride = (iOverride == null) ? groupOverride : iOverride;
            AudioSource source = GetSource();
            appliedOverride.ApplyOverride(source);

            AssetReference clipReference = (groupOverride.IdentifyByFileName) ? group.ChooseClip(soundPath) : group.ChooseRandom();
            AudioCallback callback = new AudioCallback(source);

            return callback;
        }

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
            foreach (var pair in cache)
            {
                GroupCache[pair.Key] = pair.Value;
            }
        }

        private void GenerateSources(GameObject parent, int count)
        {
            for (int i = 0; i < count; i++)
            {
                GameObject sourceObject = new GameObject($"{SOURCE_OBJECT_NAME} ({i})");
                AudioSource source = sourceObject.AddComponent<AudioSource>();
                sourceObject.SetActive(false);
                _availableSources.Add(source);
                sourceObject.transform.parent = parent.transform;
            }
        }

        private AudioSource GetSource()
        {
            if (_availableSources.Count == 0)
            {
                int count = Mathf.FloorToInt(_unavailableSources.Count * _sourceGrowthRate - _unavailableSources.Count);

                GenerateSources(_sourceParent, count);
            }

            AudioSource source = _availableSources[0];
            _availableSources.RemoveAt(0);
            _unavailableSources.Add(source);
            source.gameObject.SetActive(true);

            return source;
        }

        private void RestashSource(AudioSource source)
        {
            _unavailableSources.Remove(source);
            source.gameObject.SetActive(false);
            _availableSources.Add(source);
        }
    }
}