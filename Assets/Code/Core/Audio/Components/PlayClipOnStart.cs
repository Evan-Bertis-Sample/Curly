using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CurlyUtility;
using CurlyCore.CurlyApp;
using CurlyCore.Audio;

public class PlayClipOnStart : MonoBehaviour
{
    [AudioPath] public string AudioPath;

    private void Start()
    {
        App.Instance.AudioManager.PlayOneShot(AudioPath, default, null,
        callback =>
        {
            callback.OnAudioStart += source => Log(source, "AUDIO START");
            callback.OnAudioEnd += source => Log(source, "AUDIO END");
        });
    }

    private void Log(AudioSource source, string message)
    {
        App.Instance.Logger.Log(CurlyCore.Debugging.LoggingGroupID.APP, message);
    }
}
