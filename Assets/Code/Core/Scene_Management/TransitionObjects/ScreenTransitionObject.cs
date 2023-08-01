using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace CurlyCore.SceneManagement
{
    public abstract class ScreenTransitionObject : ScriptableObject, ISceneTransition
    {
        public abstract void EndAnimation(Canvas screenCanvas);
        public abstract void PrepareAnimation(Canvas screenCanvas);
        public abstract Task Transition(Canvas screenCanvas);
    }
}

