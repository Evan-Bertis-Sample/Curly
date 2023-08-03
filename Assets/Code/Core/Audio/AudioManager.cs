using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using CurlyCore.CurlyApp;
using CurlyUtility;

namespace CurlyCore.Audio
{
    [CreateAssetMenu(menuName = "Curly/Core/Audio Manager", fileName = "AudioManager")]
    public class AudioManager : BooterObject
    {
        [field: SerializeField, DirectoryPath] public string AudioDirectoryRoot {get; private set;}
        [field: SerializeField] public AudioOverrideGroup DefaultGroupSettings {get; private set;}
    }

}