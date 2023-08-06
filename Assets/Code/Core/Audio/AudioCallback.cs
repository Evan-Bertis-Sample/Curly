using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

using UnityEngine;

using CurlyUtility;

namespace CurlyCore.Audio
{
    public class AudioCallback
    {
        public Action OnAudioEnd;

        public AudioCallback(AudioSource source)
        {
            OnAudioEnd = null;
            AudioCallback callback = this;
            Task.Run(async () => await callback.WaitForClip(source));
        }

        public async Task WaitForClip(AudioSource source)
        {
            await TaskUtility.WaitUntil(() => source.isPlaying == false );
            OnAudioEnd?.Invoke();
        }
    }
}