using System.Collections;
using System.Collections.Generic;
using System;

using UnityEngine;

using CurlyCore.Debugging;

namespace CurlyCore.CurlyApp
{
    public class Quiter : MonoBehaviour
    {
        public event Action OnQuit;

        private void OnApplicationQuit()
        {
            App.Instance.Logger.Log(LoggingGroupID.APP, "Quitting Sequence Started");
            OnQuit?.Invoke();
        }
    }
}
