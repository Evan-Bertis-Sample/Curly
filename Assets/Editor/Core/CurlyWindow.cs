using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CurlyCore.CurlyApp;

namespace CurlyEditor.Core
{
    using UnityEngine;
    using UnityEditor;

    public class CurlyWindow : EditorWindow
    {
        [MenuItem("Curly/Shortcuts #E")]
        private static void ShowWindow()
        {
            var window = GetWindow<CurlyWindow>();
            window.titleContent = new GUIContent("Curly");
            window.Show();
        }

        private void OnGUI()
        {
            EditorGUILayout.Space(10f);
            DrawImage(AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Editor/Icons/logo_transparent.png"));
            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.LabelField("Defaults", EditorStyles.boldLabel);
            foreach(ScriptableObject de in App.Instance.Config.GlobalDefaultSystems)
            {
                if (GUILayout.Button(de.name))
                {
                   EditorUtility.OpenPropertyEditor(de);
                   Close();
                }
            }
            EditorGUILayout.EndVertical();
        }

        private void DrawImage(Sprite image)
        {
            if (image != null)
            {
                Texture2D texture = image.texture;
                float aspectRatio = (float)texture.width / texture.height;

                float availableWidth = position.width - 40;  // Considering some margins
                float calculatedHeight = availableWidth / aspectRatio;

                float maxHeight = position.height / 3;
                if (calculatedHeight > maxHeight)
                {
                    calculatedHeight = maxHeight;
                    availableWidth = calculatedHeight * aspectRatio;
                }

                Rect spriteRect = GUILayoutUtility.GetRect(availableWidth, calculatedHeight);
                GUI.DrawTexture(spriteRect, texture, ScaleMode.ScaleToFit);
            }
        }
    }
}
