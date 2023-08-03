using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityEngine;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;

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
            string[] assetGUIDs = AssetDatabase.FindAssets("t:AudioClip", new[] { audioPath });

            AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;

            foreach (string guid in assetGUIDs)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                AddressableAssetEntry assetEntry = settings.CreateOrMoveEntry(guid, settings.DefaultGroup, false, false);
                assetEntry.address = assetPath;
            }

            // For AudioOverrideGroup
            assetGUIDs = AssetDatabase.FindAssets("t:AudioOverrideGroup", new[] { audioPath });
            foreach (string guid in assetGUIDs)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                AddressableAssetEntry assetEntry = settings.CreateOrMoveEntry(guid, settings.DefaultGroup, false, false);
                assetEntry.address = assetPath;
            }

            settings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryMoved, null, true, true);
        }
    }
}
