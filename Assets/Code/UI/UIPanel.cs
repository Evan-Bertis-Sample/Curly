using System.Collections;
using System.Collections.Generic;
using CurlyCore.Saving;
using UnityEngine;
using UnityEngine.Events;

namespace CurlyCore.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class UIPanel : MonoBehaviour
    {
        [field: Header("Animations")]
        [field: SerializeField] public UIAnimation AnimationIn {get; private set;}
        [field: SerializeField] public UIAnimation AnimatonOut {get; private set;}

        [field: Header("Events")]
        [field: SerializeField] private UnityEvent<RectTransform> _onPanelCreated;
        [field: SerializeField] private UnityEvent<RectTransform> _onPanelTransitionIn;
        [field: SerializeField] private UnityEvent<RectTransform> _onPanelDelete;
        [field: SerializeField] private UnityEvent<RectTransform> _onPanelTransitonOut;

        [field: SerializeField] public UIManager Manager;
        [field: SerializeField] public FactDictionary Facts {get; private set;}

        public void SetData(UIManager manager, FactDictionary data)
        {
            Manager = manager;
            Facts = data;
        }
    }
}
