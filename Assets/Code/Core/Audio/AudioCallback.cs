using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

using UnityEngine;

using CurlyUtility;
using CurlyCore.CurlyApp;

namespace CurlyCore.Audio
{
    public class AudioCallback
    {
        public Action<AudioSource> OnAudioStart;
        public Action<AudioSource> OnAudioEnd;

        public AudioCallback(AudioSource source)
        {
            App.Instance.CoroutineRunner.StartGlobalCoroutine(WaitForClip(source));
        }

        public IEnumerator WaitForClip(AudioSource source)
        {
            yield return new WaitUntil(() => source.isPlaying);

            OnAudioStart?.Invoke(source);

            yield return new WaitUntil(() => source.isPlaying == false);

            OnAudioEnd?.Invoke(source);

            if (OnAudioEnd != null)
            {
                foreach (var d in OnAudioEnd.GetInvocationList())
                    OnAudioEnd -= d as Action<AudioSource>;
            }

            if (OnAudioStart != null)
            {
                foreach (var d in OnAudioStart.GetInvocationList())
                    OnAudioStart -= d as Action<AudioSource>;
            }
        }
    }
}