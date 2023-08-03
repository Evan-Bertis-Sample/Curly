using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using System;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;

using CurlyCore.CurlyApp;
using CurlyUtility;
using CurlyCore.Debugging;

namespace CurlyCore.Audio
{
    [CreateAssetMenu(menuName = "Curly/Core/Audio Manager", fileName = "AudioManager")]
    public class AudioManager : BooterObject
    {
        [field: SerializeField, DirectoryPath] public string AudioDirectoryRoot { get; private set; }
        [field: SerializeField] public string ReplacementPath { get; private set; } = "Audio";
        [field: SerializeField] public string OverrideGroupAddressName { get; private set; } = "Override.asset";
        [field: SerializeField] public AudioOverrideGroup DefaultGroupSettings { get; private set; }

        private Dictionary<AudioClip, AudioOverrideGroup> _groupByReference = new Dictionary<AudioClip, AudioOverrideGroup>();

        public override void OnBoot(App app, Scene scene)
        {
            // bootin bootin bootin
            CreateCacheAsync();
        }

        private async Task CreateCacheAsync()
        {
            // Cache the mappings between AssetReferences and Override Groups
            List<AudioOverrideGroup> overrideGroups = new List<AudioOverrideGroup>();

            // Load AudioOverrideGroup assets
            await LoadAssetAsync<AudioOverrideGroup>(ReplacementPath, overrideGroups.Add);

            // Create a dictionary to store AudioClips
            Dictionary<string, HashSet<AssetReference>> audioDirectoriesToReferencePath = new Dictionary<string, HashSet<AssetReference>>();
            List<AssetReference> audioClipReferences = await LoadAudioClipReferencesInDirectoryAsync(ReplacementPath);
            foreach(AssetReference reference in audioClipReferences)
            {
                App.Instance.Logger.Log(LoggingGroupID.APP, reference.RuntimeKey.ToString());
            }
        }

        private Task LoadAssetAsync<T>(string address, Action<T> onLoad)
        {
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();

            Addressables.LoadAssetsAsync<T>(address, onLoad).Completed += handle =>
            {
                if (handle.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
                {
                    tcs.SetResult(true);
                }
                else
                {
                    tcs.SetException(new Exception($"Failed to load assets of type {typeof(T)}: {handle.OperationException}"));
                }
            };

            return tcs.Task;
        }

        public Task<List<AssetReference>> LoadAudioClipReferencesInDirectoryAsync(string directoryPath)
        {
            TaskCompletionSource<List<AssetReference>> tcs = new TaskCompletionSource<List<AssetReference>>();
            List<AssetReference> audioClipReferences = new List<AssetReference>();

            // Load locations of all AudioClip assets.
            Addressables.LoadResourceLocationsAsync(typeof(AudioClip)).Completed += handle =>
            {
                if (handle.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
                {
                    foreach (var location in handle.Result)
                    {
                        string locationDirectory = Path.GetDirectoryName(location.InternalId);

                        // Check if the location's directory starts with the provided directory.
                        if (locationDirectory.StartsWith(directoryPath))
                        {
                            // Create an AssetReference from the asset's GUID and add it to the list.
                            AssetReference assetReference = new AssetReference(location.PrimaryKey);
                            audioClipReferences.Add(assetReference);
                        }
                    }

                    tcs.SetResult(audioClipReferences);
                }
                else
                {
                    tcs.SetException(new System.Exception($"Failed to load AudioClip locations: {handle.OperationException}"));
                }
            };

            return tcs.Task;
        }

    }
}