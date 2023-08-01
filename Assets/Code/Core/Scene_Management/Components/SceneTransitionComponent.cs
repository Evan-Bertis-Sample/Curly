using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.CurlyApp;

namespace Core.SceneManagement.Components
{
    public class SceneTransitionComponent : MonoBehaviour
    {
        public string TargetScene;
        public bool UseDefaultTransitions = true;

        public void TransitionScene()
        {
            App.Instance.SceneMaster.LoadSceneAsync(TargetScene, UseDefaultTransitions);
        }
    }
}
