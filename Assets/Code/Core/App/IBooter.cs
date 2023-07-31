using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.CurlyApp
{
    public interface IBooter
    {
        /// <summary>
        /// Called on game startup, given that that the AppConfig knows of it's existence
        /// </summary>
        /// <param name="app"> The App object calling the Boostrap </param>
        /// <param name="scene"> The Scene that the game is starting in</param>
        public void OnBoot(App app, Scene scene);
    }

}