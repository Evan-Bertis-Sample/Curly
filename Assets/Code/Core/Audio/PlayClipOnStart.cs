using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CurlyUtility;
using CurlyCore.CurlyApp;
using CurlyCore.Audio;

public class PlayClipOnStart : MonoBehaviour
{
    [DirectoryPath] public string AudioPath;

    private void Start()
    {
        App.Instance.AudioManager.PlayOneShot(AudioPath);
    }


    private void Log(AudioSource source)
    {
        App.Instance.Logger.Log(CurlyCore.Debugging.LoggingGroupID.APP, "AUDIO END");
    }
}
