using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CurlyUtility;
using CurlyCore.CurlyApp;
using CurlyCore.Audio.Transitions;

namespace CurlyCore.Audio
{
    public class BackgroundMusic : MonoBehaviour
    {
        [AudioPath] public string MusicPath;
        public AudioTransitionObject TransitionIn;
        public AudioTransitionObject TransitionOut;
        public AudioOverride AudioOverride;

        private void Awake()
        {
            App.Instance.AudioManager.SetBackgroundMusicAsync(MusicPath, TransitionIn, TransitionOut, AudioOverride);
        }
    }
}
