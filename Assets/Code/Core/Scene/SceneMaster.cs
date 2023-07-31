using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Core.CurlyApp;

namespace Core.SceneManagement
{
    [CreateAssetMenu(menuName = "Core/Scene Master", fileName = "SceneMaster")]
    public sealed class SceneMaster : BooterObject
    {
        public List<int> test;
        
        public override void OnBoot(App app, Scene scene)
        {
            // We be bootin
        }
    }
}
