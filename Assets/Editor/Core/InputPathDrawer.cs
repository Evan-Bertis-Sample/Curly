using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEditor;
using UnityEngine.InputSystem;

using CurlyCore.CurlyApp;
using CurlyCore.Input;
using CurlyEditor.Utility;

namespace CurlyEditor.Core
{
    [CustomPropertyDrawer(typeof(InputPathAttribute))]
    public class InputPathDrawer : SearchBarDrawer
    {
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            base.OnGUI(position, property, label);

            if (!App.Instance.InputManager.IsInputAssigned(property.stringValue))
            {
                Rect warningPosition = new Rect(position.x, position.y + _standardPropertyHeight * 1.25f, position.width, _standardPropertyHeight * 1.75f);
                EditorGUI.HelpBox(warningPosition, $"Input: '{property.stringValue}' is not assigned in Input Manager!", MessageType.Warning);
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (App.Instance.InputManager.IsInputAssigned(_propertyValue)) return _standardPropertyHeight;
            return _standardPropertyHeight * 3f;
        }

        protected override void ButtonClicked(Rect browserPosition)
        {
            Leaf<string> browserContent = GenerateBrowserContent();
            DropDownBrowser browser = new DropDownBrowser(browserContent);
            browser.PathUpdate += UpdateProperty;
            PopupWindow.Show(browserPosition, browser);
        }

        private Leaf<string> GenerateBrowserContent()
        {
            if (App.Instance.InputManager.MasterActionAsset == null) throw new System.Exception("Action map is not defined.");
            IEnumerable<InputActionMap> actionMaps = App.Instance.InputManager.MasterActionAsset.actionMaps;

            List<Leaf<string>> children = new List<Leaf<string>>();

            foreach (InputActionMap map in actionMaps)
            {
                List<string> mapChildren = new List<string>();
                foreach (InputAction action in map.actions)
                {
                    mapChildren.Add(action.name);
                }
                children.Add(new Leaf<string>(map.name, mapChildren, null));
            }

            return new Leaf<string>(null, children);
        }

        private void UpdateProperty(string value)
        {
            _propertyValue = value;
        }
    }
}