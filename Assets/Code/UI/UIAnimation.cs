using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CurlyUtility;
using System.Threading.Tasks;

namespace CurlyCore.UI
{
    public abstract class UIAnimation : ScriptableObject, ITransition<RectTransform>
    {
        public abstract void EndTransition(RectTransform value);
        public abstract void PrepareTransition(RectTransform value);
        public abstract Task Transition(RectTransform value);
    }
}
