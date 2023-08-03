using System.Linq;

using UnityEngine;
using UnityEngine.SceneManagement;

using CurlyCore.SceneManagement;
using CurlyCore.Input;
using CurlyCore.Debugging;
using CurlyCore.Audio;

namespace CurlyCore.CurlyApp
{
    /// <summary>
    /// A singleton devoted to initalizing the game no matter what scene is the first scene
    /// </summary>
    public sealed class App
    {
        #region Singleton setup
        private static App _instance;
        public static App Instance
        {
            get
            {
                if (_instance == null) _instance = new App();
                return _instance;
            }
        }
        #endregion

        private AppConfig _config;

        // Fields
        public static string ConfigPath = "Core_Config/AppConfig";
        public AppConfig Config => _config;
        public SceneMaster SceneMaster => _config.SceneMaster;
        public InputManager InputManager => _config.InputManager;
        public GroupLogger Logger => _config.Logger;
        public AudioManager AudioManager => _config.AudioManger;

        private App()
        {
            _config = Resources.Load<AppConfig>(App.ConfigPath);
        }

        /// <summary>
        /// Adds all dependencies on gamestartup and performs level loading logic if the starting scene is a level
        /// </summary>
        public void InitializeGame()
        {
            Logger.Log(LoggingGroupID.APP, "Initializing Game!");
            if (_config == null)
            {
                Debug.LogError("Could not find App Config!");
                return;
            }

            Boot();
            // Spawn objects
            DressScene();

            Scene startingScene = SceneManager.GetActiveScene();
        }

        private void Boot()
        {
            Scene startingScene = SceneManager.GetActiveScene();
            // Instantiate Quitter
            GameObject quit = new GameObject("Quitter");
            GameObject.DontDestroyOnLoad(quit.transform.root.gameObject);

            Quiter quitterComponent = quit.AddComponent<Quiter>();

            foreach (BooterObject b in _config.Booters)
            {
                if (b == null) continue;
                b.OnBoot(this, startingScene);
                quitterComponent.OnQuit += () => b.OnQuit(this, startingScene);
            }
        }

        /// <summary>
        /// Adds all the persistent gameobjects to the scene upon game load. These objects are added to the DDOL Scene
        /// </summary>
        private void DressScene()
        {
            if (_config.PersistentObjects == null) return;

            foreach (GameObject obj in _config.PersistentObjects)
            {
                // Instantiate and put into DDOL scene
                if (obj == null) continue;
                Logger.Log(LoggingGroupID.APP, $"Adding {obj.name}");
                GameObject instance = GameObject.Instantiate(obj, Vector3.zero, Quaternion.identity);
                GameObject.DontDestroyOnLoad(instance.transform.root.gameObject);
            }
        }
    }
}
