using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

using CurlyUtility;

namespace CurlyEditor.Utility
{
    [CustomPropertyDrawer(typeof(DirectoryPath))]
    public class DirectoryPathDrawer : SearchBarDrawer
    {
        protected override void ButtonClicked(Rect buttonPosition)
        {
            _propertyValue = EditorUtility.OpenFolderPanel("Choose Directory", "", "");
        }
    }

    [CustomPropertyDrawer(typeof(FilePath))]
    public class FilePathDrawer : SearchBarDrawer
    {
        protected override void ButtonClicked(Rect buttonPosition)
        {
            _propertyValue = EditorUtility.OpenFilePanel("Choose File", "", "");
        }
    }
}
