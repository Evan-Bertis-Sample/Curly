using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

using UnityEngine;
using UnityEngine.SceneManagement;

using CurlyCore.CurlyApp;
using CurlyUtility;

namespace CurlyCore.Debugging
{
    [CreateAssetMenu(menuName = "Curly/Core/Logger", fileName = "Logger")]
    public class GroupLogger : BooterObject
    {
        [field: Header("File Logging Configuration")]
        [field: SerializeField] public bool LogToFile {get; private set;}
        [field: SerializeField, DirectoryPath(true)] public string LoggingPath {get; private set;}

        [field: Header("Logging Configuration")]
        [field: SerializeField, FilePath] public string LoggingEnumPath {get; private set;}
        [field: SerializeField] public List<LoggingGroup> LoggingGroups {get; private set;}

        private bool _loggedThisSession;
        [SerializeField, TextArea] private string _logContents;
        private const string _LAST_LOG_FILE = "last_session.txt";

        public override void OnBoot(App app, Scene scene)
        {
            _loggedThisSession = false;

            #if UNITY_EDITOR
            // Set up log contents
            if (LogToFile) SetupLogContents();
            #endif

            Log(LoggingGroupID.APP, "test");
        }

        public void Log(LoggingGroupID type, object message, LogType logType = LogType.Log)
        {
            #if UNITY_EDITOR
            _loggedThisSession = true;
            LoggingGroup group = LoggingGroups[(int)type];
            string logColor = ColorUtility.ToHtmlStringRGB(group.LoggingColor);
            string prefix = $"<b><color=#{logColor}>{group.GroupName}: </color></b>";

            Debug.unityLogger.Log(logType, prefix + message.ToString());

            _logContents += $"{group.GroupName} ({DateTime.Now.ToString("HH:mm:ss")}): {message.ToString()}\n";
            #else

            return;

            #endif
        }

        public override void OnQuit(App app, Scene scene)
        {
            Log(LoggingGroupID.APP, "Finishing Logging Session");
            if (LogToFile == false || _loggedThisSession == false) return;

            string lastLogPath = $"{LoggingPath}/{_LAST_LOG_FILE}";
            if (File.Exists(lastLogPath))
            {
                // Move contents from old last log to new last log
                string oldContents = File.ReadAllText(lastLogPath);
                string prevPath = $"{LoggingPath}/prev.txt";
                Log(LoggingGroupID.APP, "Moving old logging contents to new file");
                File.WriteAllText(prevPath, oldContents);
            }
            else
            {
                // We need to create a file
                using (File.Create(lastLogPath)) { }
            }

            File.WriteAllText(lastLogPath, _logContents);

        }

        private void SetupLogContents()
        {
            DateTime time = DateTime.Now;
            _logContents = $"LOG SESSION ({time})\n";
            _logContents += string.Concat(System.Linq.Enumerable.Repeat("-", 40));
            _logContents += "\n";
        }
    }

}