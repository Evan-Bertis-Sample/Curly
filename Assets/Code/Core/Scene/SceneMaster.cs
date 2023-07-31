using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Core.CurlyApp;
using Utility;

namespace Core.SceneManagement
{
    [CreateAssetMenu(menuName = "Core/Scene Master", fileName = "SceneMaster")]
    public sealed class SceneMaster : BooterObject
    {   
        // Configuration
        [field: SerializeField] public string ScenePath;

        public override void OnBoot(App app, Scene scene)
        {
            CreateScreenCanvas();
        }

        public async void LoadSceneAsync(string sceneName, bool useDefault)
        {

        }

        /// <summary>
        /// Loads a scene async using the transition in and transition out animations
        /// </summary>
        /// <param name="sceneName"> The desired scene to transition into </param>
        /// <param name="transitionIn"> The transition played at the beginning of the transition </param>
        /// <param name="transitionOut"> The transition played leaving the transition </param>
        /// <returns></returns>
        public async void LoadSceneAsync(string sceneName, ISceneTransition transitionIn = null, ISceneTransition transitionOut = null)
        {
            Debug.Log($"Loading scene {sceneName}");

            AsyncOperation loadingOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            loadingOperation.allowSceneActivation = false;

            // ENTER LOAD
            List<Task> loadingTasks = new List<Task>();
            // Add actual loading task
            loadingTasks.Add(TaskUtility.WaitUntil(() =>
            {
                Debug.Log($"Loading... {loadingOperation.progress * 100}%");
                return loadingOperation.isDone;
            }));

            Canvas screenCanvas = CreateScreenCanvas();
            DontDestroyOnLoad(screenCanvas);

            if (transitionIn != null) loadingTasks.Add(transitionIn.Play(screenCanvas));

            await Task.WhenAll(loadingTasks);

            // Finished loading! 
            if (transitionOut != null) await transitionOut.Play(screenCanvas);

            // Destroy the canvas
            Destroy(screenCanvas);
        }

        private Canvas CreateScreenCanvas()
        {
            // Create a new GameObject in the scene
            GameObject canvasObject = new GameObject("Transition Canvas");

            // Add a Canvas component to the GameObject
            Canvas canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;

            // Ensure that the Canvas is on top of all other UI elements
            canvas.sortingOrder = short.MaxValue;

            // Add a CanvasScaler component to ensure the Canvas covers the full screen
            CanvasScaler scaler = canvasObject.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;

            // Add a GraphicRaycaster component to enable input events
            canvasObject.AddComponent<GraphicRaycaster>();

            // Create a RectTransform and set it to cover the whole screen
            RectTransform rect = canvas.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(0.5f, 0.5f);
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.sizeDelta = Vector2.zero;

            return canvas;
        }
    }
}
