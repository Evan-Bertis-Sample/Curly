using System.Collections;
using System.Collections.Generic;
using CurlyCore.Saving;
using UnityEditor;
using UnityEngine;

namespace CurlyCore.UI
{
    [RequireComponent(typeof(Canvas))]
    public class UIManager : MonoBehaviour
    {
        [field: SerializeField] public Canvas RootCanvas { get; private set; }
        [field: SerializeField] public RectTransform RootTransform {get; private set;}
        [field: SerializeField] public Stack<UIPanel> Panels { get; private set; } = new Stack<UIPanel>();
        [field: SerializeField] public UIPanel StartingPanel { get; private set; }
        [field: SerializeField] public bool PushPanelOnStart { get; private set; }

        private void Start()
        {
            RootCanvas = GetComponent<Canvas>();
            RootTransform = GetComponent<RectTransform>();
            if (PushPanelOnStart) StartUI();

        }

        public void StartUI()
        {
            if (Panels.Count != 0)
            {
                Debug.LogWarning("Cannot Start UI! UI Has already been started");
            }

            PushPanel(StartingPanel);
        }

        public void PushPanel(UIPanel panel, FactDictionary data = null)
        {
            if (panel == null)
            {
                Debug.LogWarning($"Cannot push panel onto Manager {name}");
            }

            UIPanel instance = Instantiate(panel);
            Panels.Push(instance);
            instance.SetData(this, data);

            RectTransform instanceTransform = instance.GetComponent<RectTransform>();
            if (instanceTransform == null)
            {
                Debug.LogWarning($"Please ensure that the UIPanel {panel} is a UI element!");
                Destroy(instance);
                return;
            }

            instanceTransform.parent = RootTransform;
        }

        public UIPanel PopPanel()
        {
            UIPanel top = Panels.Pop();
            return top;
        }
    }
}
