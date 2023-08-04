using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;

using CurlyCore.Audio;
using CurlyEditor.Utility;
using CurlyUtility;

namespace CurlyEditor.Core
{
    [CustomEditor(typeof(AudioManager))]
    public class AudioManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            AudioManager manager = target as AudioManager;
            if (manager.AudioDirectoryRoot != "" && GUILayout.Button("Build Adressables")) BuildAddressables(manager);
        }

        private void BuildAddressables(AudioManager manager)
        {
            string audioPath = manager.AudioDirectoryRoot;

            AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;

            // Find or create the "Audio" group
            AddressableAssetGroup audioGroup = settings.FindGroup(manager.AUDIO_GROUP_NAME);
            if (audioGroup == null)
            {
                audioGroup = settings.CreateGroup(manager.AUDIO_GROUP_NAME, false, false, true, new List<AddressableAssetGroupSchema>());
            }

            // Process the root directory
            List<AddressableAssetEntry> overrideEntries = ProcessDirectory(manager, audioPath, settings, audioGroup);
            CreateCache(manager, overrideEntries);

            settings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryMoved, null, true, true);
        }

        /// <summary>
        /// Proccess the Audio directory
        /// </summary>
        /// <param name="manager"> The AudioManager that dictates how the Directory is processed </param>
        /// <param name="directoryPath"> The path of the directory </param>
        /// <param name="settings"> Addressables settings </param>
        /// <param name="group"> The Addressables group to place the processed entries </param>
        /// <returns> A List of AudioOverride Entries -- used for building the cache </returns>
        private List<AddressableAssetEntry> ProcessDirectory(AudioManager manager, string directoryPath, AddressableAssetSettings settings, AddressableAssetGroup group)
        {
            // Get the GUIDs of the directory itself, any AudioOverrideGroups and any AudioClips in this directory
            string[] childDirectories = Directory.GetDirectories(directoryPath, "*", SearchOption.TopDirectoryOnly);

            // Now recursively process any subdirectories
            string parentGuid = AssetDatabase.AssetPathToGUID(directoryPath);
            AddressableAssetEntry parentEntry = settings.FindAssetEntry(parentGuid);
            if (parentEntry != null && childDirectories.Length > 0)
            {
                parentEntry.IsFolder = true;
            }

            List<AddressableAssetEntry> overrideEntries = new List<AddressableAssetEntry>();
            // Process each GUID
            foreach (string childDirectory in childDirectories)
            {
                string directory = childDirectory.Replace("\\", "/");
                string guid = AssetDatabase.AssetPathToGUID(directory);
                AddressableAssetEntry assetEntry = settings.CreateOrMoveEntry(guid, group, false, false);
                assetEntry.address = directory;

                if (parentEntry != null)
                {
                    Debug.Log($"Processed Directory '{directory}' as child of Directory '{directoryPath}'");
                    assetEntry.IsSubAsset = true;
                    assetEntry.ParentEntry = parentEntry;
                    assetEntry.IsFolder = true;

                    if (parentEntry.SubAssets != null) parentEntry.SubAssets.Add(assetEntry);
                    else parentEntry.SubAssets = new List<AddressableAssetEntry>() { assetEntry };

                    assetEntry.labels.Clear();
                    assetEntry.SetLabel(directoryPath, true, true, true);
                    assetEntry.SetLabel("AudioDirectory", true, true, true);

                    ProcessDirectoryContent<AudioClip>(directory, manager.CLIP_LABEL, settings, group);
                    overrideEntries.AddRange(ProcessDirectoryContent<AudioOverrideGroup>(directory, manager.OVERRIDE_LABEL, settings, group));
                }

                overrideEntries.AddRange(ProcessDirectory(manager, directory, settings, group));
            }

            return overrideEntries;
        }

        private List<AddressableAssetEntry> ProcessDirectoryContent<T>(string directoryPath, string label, AddressableAssetSettings settings, AddressableAssetGroup group) where T : Object
        {
            T[] clips = AssetUtility.GetAssetsAtPath<T>(directoryPath);
            List<AddressableAssetEntry> entries = new List<AddressableAssetEntry>();
            foreach (T clip in clips)
            {
                string path = AssetDatabase.GetAssetPath(clip);
                string guid = AssetDatabase.AssetPathToGUID(path);
                AddressableAssetEntry clipEntry = settings.CreateOrMoveEntry(guid, group, false, false);
                entries.Add(clipEntry);
                clipEntry.labels.Clear();
                clipEntry.SetLabel(directoryPath, true, true);
                clipEntry.SetLabel(label, true, true);
            }

            return entries;
        }

        private async void CreateCache(AudioManager manager, List<AddressableAssetEntry> overrideEntries)
        {
            Dictionary<AssetReference, AudioOverrideGroup> mapping = new Dictionary<AssetReference, AudioOverrideGroup>();

            foreach (AddressableAssetEntry entry in overrideEntries)
            {
                string assetPath = entry.AssetPath;
                string guid = AssetDatabase.AssetPathToGUID(assetPath);
                AssetReference assetReference = new AssetReference(guid);

                // Load the AudioOverrideGroup
                AudioOverrideGroup group = await Addressables.LoadAssetAsync<AudioOverrideGroup>(assetReference).Task;

                if (group != null)
                {
                    mapping[assetReference] = group;
                }
            }

            manager.SetCache(mapping);
        }
    }
}
