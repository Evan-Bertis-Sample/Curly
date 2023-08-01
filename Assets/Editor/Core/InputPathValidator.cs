using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.IO;

using UnityEngine;
using UnityEditor;

using CurlyCore.Input;
using CurlyCore.CurlyApp;

#if UNITY_EDITOR
namespace CurlyEditor.Core
{
    public class InputPathValidator
    {
        // Use a dictionary to hold invalid values and the fields that hold them
        private static Dictionary<string, List<(Object obj, FieldInfo field)>> invalidValues = new Dictionary<string, List<(Object obj, FieldInfo field)>>();

        [MenuItem("Curly/Tools/Validate InputPaths")]
        public static void ValidateInputPaths()
        {
            invalidValues.Clear();

            string[] guids = AssetDatabase.FindAssets("t:Object");  // This finds all assets

            foreach (var guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                Object obj = AssetDatabase.LoadAssetAtPath<Object>(path);
                if (obj == null) continue;

                var type = obj.GetType();
                FieldInfo[] allFields = type.GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                foreach (FieldInfo field in allFields)
                {
                    if (InputPathAttribute.IsDefined(field, typeof(InputPathAttribute)))
                    {
                        bool isValid = ValidateInputPath(obj, field);
                        if (!isValid)
                        {
                            string invalidValue = (string)field.GetValue(obj);
                            Debug.LogError($"Invalid InputPath at {path}, field {field.Name}, value: {invalidValue}");

                            if (!invalidValues.ContainsKey(invalidValue))
                            {
                                invalidValues[invalidValue] = new List<(Object obj, FieldInfo field)>();
                            }

                            invalidValues[invalidValue].Add((obj, field));
                        }
                    }
                }
            }
            if (invalidValues.Count == 0)
            {
                Debug.Log("InputPaths validated!");
            }

            foreach (string invalidValue in invalidValues.Keys)
            {
                InputPathCorrectionWindow.Init(invalidValue, invalidValues[invalidValue]);
            }
        }

        private static bool ValidateInputPath(Object obj, FieldInfo field)
        {
            string fieldValue = (string)field.GetValue(obj);
            return App.Instance.InputManager.IsInputAssigned(fieldValue);
        }
    }

}
#endif