using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CurlyCore.CurlyApp;
using CurlyCore.Debugging;

namespace CurlyCore.Saving
{
    public class SaveData
    {
        public FactDictionary Facts { get; private set; }

        public SaveData()
        {
            Facts = new FactDictionary();
        }
        
        public void Bind<T>(string factName, ref T value, T fallback)
        {
            try
            {
                value = (T)Facts[factName];
            }
            catch (System.Exception error)
            {
                App.Instance.Logger.Log(LoggingGroupID.APP, error, LogType.Warning);
                value = fallback;
                Facts[factName] = fallback;
            }
        }
    }
}
