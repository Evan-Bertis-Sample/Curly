using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CurlyCore.Audio
{
    [CreateAssetMenu(fileName = "AudioOverrideGroup", menuName = "Curly/Audio/AudioOverrideGroup")]
    public class AudioOverrideGroup : ScriptableObject
    {
        [field: SerializeField] public bool IdentifyByFileName {get; private set;} // If this flag is true, then we consider each sound file in the directory as a different sfx -- they are identified by name, not directory
    }
}