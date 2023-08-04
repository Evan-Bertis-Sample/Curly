using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using UnityEngine;
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
            ProcessDirectory(audioPath, settings, audioGroup);

            settings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryMoved, null, true, true);
        }

        private void ProcessDirectory(string directoryPath, AddressableAssetSettings settings, AddressableAssetGroup group)
        {
            // Create a new group for this directory
            // AddressableAssetGroup directoryGroup = settings.CreateGroup(directoryPath, false, false, false, new List<AddressableAssetGroupSchema>(), typeof(BundledAssetGroupSchema));

            // Get the GUIDs of the directory itself, any AudioOverrideGroups and any AudioClips in this directory
            string[] childDirectories = Directory.GetDirectories(directoryPath, "*", SearchOption.TopDirectoryOnly);
            // string[] overrideGUIDs = AssetDatabase.FindAssets("t:AudioOverrideGroup", new[] { directoryPath });
            // string[] audioGUIDs = AssetDatabase.FindAssets("t:AudioClip", new[] { directoryPath });

            // // Combine all these GUIDs into one array
            // string[] contentGUIDs = overrideGUIDs.Concat(audioGUIDs).ToArray();

            // Now recursively process any subdirectories
            string parentGuid = AssetDatabase.AssetPathToGUID(directoryPath);
            AddressableAssetEntry parentEntry = settings.FindAssetEntry(parentGuid);


            // Process each GUID
            foreach (string childDirectory in childDirectories)
            {
                string directory = childDirectory.Replace("\\", "/");
                string guid = AssetDatabase.AssetPathToGUID(directory);
                AddressableAssetEntry assetEntry = settings.CreateOrMoveEntry(guid, group, false, false);
                assetEntry.address = directory;

                if (parentEntry != null)
                {
                    Debug.Log($"Processed Directory {directory} as Child of Directory {directoryPath}");
                    assetEntry.IsSubAsset = true;
                    assetEntry.ParentEntry = parentEntry;
                    parentEntry.IsFolder = true;

                    if (parentEntry.SubAssets != null) parentEntry.SubAssets.Add(assetEntry);
                    else parentEntry.SubAssets = new List<AddressableAssetEntry>() { assetEntry };

                    assetEntry.labels.Clear();
                    assetEntry.SetLabel(directoryPath, true, true);
                }

                ProcessDirectory(directory, settings, group);
            }
        }


    }
}
