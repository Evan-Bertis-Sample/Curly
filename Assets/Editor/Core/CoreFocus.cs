using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using CurlyCore.CurlyApp;

#if UNITY_EDITOR
namespace CurlyEditor
{
    public sealed class CoreFocus
    {
        [MenuItem("Curly/Core/Scene Master")]
        public static void FocusSceneMaster()
        {
            if (App.Instance.SceneMaster == null) return;

            // Selection.activeObject = App.Instance.SceneMaster;
            EditorUtility.OpenPropertyEditor(App.Instance.SceneMaster);
        }

        [MenuItem("Curly/Core/Input Manager")]
        public static void FocusInputManager()
        {
            if (App.Instance.InputManager == null) return;

            // Selection.activeObject = App.Instance.InputManager;
            EditorUtility.OpenPropertyEditor(App.Instance.InputManager);
        }

        [MenuItem("Curly/Core/Logger")]
        public static void FocusLogger()
        {
            if (App.Instance.Logger == null) return;

            // Selection.activeObject = App.Instance.Logger;
            EditorUtility.OpenPropertyEditor(App.Instance.Logger);
        }

        [MenuItem("Curly/Core/Audio Manager")]
        public static void FocusAudioManger()
        {
            if (App.Instance.AudioManager == null) return;

            // Selection.activeObject = App.Instance.AudioManager;
            EditorUtility.OpenPropertyEditor(App.Instance.AudioManager);
        }
    }
}
#endif