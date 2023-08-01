using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using CurlyCore.SceneManagement;

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

        private App()
        {
            _config = Resources.Load<AppConfig>(App.ConfigPath);
        }

        /// <summary>
        /// Adds all dependencies on gamestartup and performs level loading logic if the starting scene is a level
        /// </summary>
        public void InitializeGame()
        {
            Debug.Log("Initializing Game!");
            if (_config == null)
            {
                Debug.LogError("Could not find App Config!");
                return;
            }

            Boot();
            // Spawn objects
            DressScene();

            Scene startingScene = SceneManager.GetActiveScene();
            if (IsLevelScene(startingScene))
            {
                // Do some level logic
            }
            else
            {
                // Do some menu logic
            }
        }

        private void Boot()
        {
            Scene startingScene = SceneManager.GetActiveScene();
            foreach (IBooter b in _config.Booters)
            {
                b?.OnBoot(this, startingScene);
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
                Debug.Log($"Adding {obj.name}");
                GameObject instance = GameObject.Instantiate(obj, Vector3.zero, Quaternion.identity);
                GameObject.DontDestroyOnLoad(instance.transform.root.gameObject);
            }
        }

        private bool IsLevelScene(Scene currentScene)
        {
            if (_config.NonGameplayScenes == null) return false;

            return !_config.NonGameplayScenes.Any(s => s.name == currentScene.name);
        }
    }
}
