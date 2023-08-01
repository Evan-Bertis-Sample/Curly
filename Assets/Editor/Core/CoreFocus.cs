using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Core.CurlyApp;

#if UNITY_EDITOR
namespace CurlyEditor
{
    public sealed class CoreFocus
    {
        [MenuItem("Curly/Core/SceneMaster")]
        public static void FocusSceneMaster()
        {
            if (App.Instance.SceneMaster == null) return;

            Selection.activeObject = App.Instance.SceneMaster;
        }
    }
}
#endif