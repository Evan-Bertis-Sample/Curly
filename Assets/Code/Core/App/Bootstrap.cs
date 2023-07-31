using UnityEngine;

namespace Core.CurlyApp
{
    /// <summary>
    // Bootstrap Class
    /// </summary>
    public sealed class Bootstrap
    {
        /// <summary>
        /// Initialization method that runs before any scene has loaded
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Initialize()
        {
            App.Instance.InitializeGame();
        }
    }
}