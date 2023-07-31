using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
using UnityEditor;

namespace Utility
{
    public class AssetUtility
    {
        /// <summary>Gets an array of assets of type T at a given path. This path is relative to /Assets.</summary>
        /// <returns>An array of assets of type T.</returns>
        /// <param name="path">The file path relative to /Assets.</param>
        public static T[] GetAssetsAtPath<T>(string path) where T : Object
        {
            List<T> returnList = new List<T>();

            //get the contents of the folder's full path (excluding any meta files) sorted alphabetically
            IEnumerable<string> fullpaths = Directory.GetFiles(Path.GetFullPath(path)).Where(x => !x.EndsWith(".meta")).OrderBy(s => s);
            //loop through the folder contents
            foreach (string fullpath in fullpaths)
            {
                //determine a path starting with Assets
                string assetPath = FileUtil.GetProjectRelativePath(fullpath.Replace(@"\", "/"));
                //load the asset at this relative path
                Object obj = AssetDatabase.LoadAssetAtPath<T>(assetPath);
                //and add it to the list if it is of type T
                if (obj is T) { returnList.Add(obj as T); }
            }

            return returnList.ToArray();
        }
    }
}
