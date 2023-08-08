using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using UnityEngine;
using UnityEditor;

using CurlyCore.CurlyApp;
using CurlyCore.Audio;
using CurlyUtility;
using CurlyEditor.Utility;

namespace CurlyEditor.Core
{
    [CustomPropertyDrawer(typeof(AudioPath))]
    public class AudioPathDrawer : SearchBarDrawer
    {
        public AudioPathDrawer()
        {

        }

        protected override void ButtonClicked(Rect buttonPosition)
        {
            App.Instance.Logger.Log(CurlyCore.Debugging.LoggingGroupID.APP, "Audio Path");
            Leaf<string> content = GenerateContent();
            DropDownBrowser browser = new DropDownBrowser(content);
            browser.PathUpdate += UpdateProperty;
            PopupWindow.Show(buttonPosition, browser);
        }

        private Leaf<string> GenerateContent()
        {
            List<Leaf<string>> children = GetChildren(App.Instance.AudioManager.AudioDirectoryRoot);

            return new Leaf<string>(null, children);
        }

        private List<Leaf<string>> GetChildren(string path)
        {
            List<Leaf<string>> children = new List<Leaf<string>>();

            string[] subdirectories = Directory.GetDirectories(path, "*", SearchOption.TopDirectoryOnly);

            for (int i = 0; i < subdirectories.Length; i++)
            {
                string subdirectory = subdirectories[i].Replace("\\", "/");
                List<Leaf<string>> leafChildren = GetChildren(subdirectory);

                AudioOverride over = App.Instance.AudioManager.GetOverride(subdirectory);

                if (over != null && over.IdentifyByFileName)
                {
                    // Add all of the files
                    Dictionary<AudioClip, string> pathsByAsset = AssetUtility.GetAssetsInDirectory<AudioClip>(subdirectory);
                    if (leafChildren == null && pathsByAsset.Count > 0) leafChildren = new List<Leaf<string>>();
                    foreach(var pair in pathsByAsset)
                    {
                        string filename = GetLast(pair.Value);
                        Leaf<string> assetLeaf = new Leaf<string>(filename, null);
                        leafChildren.Add(assetLeaf);
                    }
                }

                string dir = GetLast(subdirectory);
                Leaf<string> child = new Leaf<string>(dir, leafChildren);

                children.Add(child);
            }

            return children;
        }

        private void UpdateProperty(string value)
        {
            _propertyValue = App.Instance.AudioManager.AudioDirectoryRoot + "/" + value;
        }

        private string GetLast(string path)
        {
            string last = path.Split('/').Last();
            Debug.Log($"Last of {path} is {last}");
            return last;
        }
    }
}
