using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CurlyCore.Audio
{
    public class AudioPath : PropertyAttribute
    {
        public AudioManager Manager;

        public AudioPath()
        {
            Manager = GlobalDefaultStorage.GetDefault(typeof(AudioManager)) as AudioManager;
        }
    }
}

