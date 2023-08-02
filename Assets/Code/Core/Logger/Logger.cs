using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

using CurlyCore.CurlyApp;
using CurlyUtility;

namespace CurlyCore.Debugging
{
    [CreateAssetMenu(menuName = "Curly/Core/Logger", fileName = "Logger")]
    public class Logger : BooterObject
    {
        [field: Header("File Logging Configuration")]
        [field: SerializeField] public bool LogToFile {get; private set;}
        [field: SerializeField, FilePath] public string LoggingPath {get; private set;}

        [field: Header("Logging Configuration")]
        [field: SerializeField, FilePath] public string LoggingEnumPath {get; private set;}
        [field: SerializeField] public List<LoggingGroup> LoggingGroups {get; private set;}

        public override void OnBoot(App app, Scene scene)
        {
            return;
        }

    }

}