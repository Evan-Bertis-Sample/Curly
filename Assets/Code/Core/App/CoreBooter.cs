using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CurlyCore.CurlyApp
{
    /// <summary>
    /// A wrapper class for the common use pattern of using an IBooter ScriptableObject
    /// </summary>
    public abstract class BooterObject : ScriptableObject, IBooter
    {
        public abstract void OnBoot(App app, Scene scene);
    }
}