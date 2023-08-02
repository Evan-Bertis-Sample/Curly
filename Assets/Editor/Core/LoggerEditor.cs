using System.IO;
using System.Linq;

using UnityEngine;
using UnityEditor;

using CurlyCore.Debugging;

namespace CurlyEditor.Core
{
    [CustomEditor(typeof(GroupLogger))]
    public class LoggerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GroupLogger logger = target as GroupLogger;
            TextAsset asset = AssetDatabase.LoadAssetAtPath<TextAsset>(logger.LoggingEnumPath);
            if (asset == null) return;
            if (GUILayout.Button("Generate Enums")) GenerateLoggerEnum(logger, logger.LoggingEnumPath);
        }

        private void GenerateLoggerEnum(GroupLogger logger, string path)
        {
            if (logger.LoggingGroups == null || logger.LoggingGroups.Count == 0) return;

            string enumValues = "";
            for (int i = 0; i < logger.LoggingGroups.Count; i++)
            {
                LoggingGroup group = logger.LoggingGroups[i];
                string value = "\t\t";
                value += group.GroupName.ToUpper();
                if (i != logger.LoggingGroups.Count - 1) value += ",\n";
                else value += "\n";

                enumValues += value;
            }

            string content =
                "namespace CurlyCore.Debugging\n" +
                "{\n" +
                    "\tpublic enum LoggingGroupID\n" +
                    "\t{\n" +
                        $"{enumValues}" +
                    "\t}\n" +
                "}";

            File.WriteAllText(path, content);
            AssetDatabase.Refresh();
        }
    }
}