using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.AddressableAssets;

using CurlyCore.CurlyApp;
using CurlyUtility;

namespace CurlyCore.Audio
{
    [CreateAssetMenu(menuName = "Curly/Core/Audio Manager", fileName = "AudioManager")]
    public class AudioManager : BooterObject
    {
        [field: SerializeField, DirectoryPath] public string AudioDirectoryRoot {get; private set;}
        [field: SerializeField] public string ReplacementPath {get; private set;} = "Audio";
        [field: SerializeField] public string OverrideGroupAddressName {get; private set;} = "Override.asset";
        [field: SerializeField] public AudioOverrideGroup DefaultGroupSettings {get; private set;}
    }

}