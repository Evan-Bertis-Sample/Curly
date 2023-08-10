using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CurlyCore.CurlyApp;
using CurlyCore.Debugging;

namespace CurlyCore.Saving
{
    [System.Serializable]
    public class SaveData
    {
        public FactDictionary Facts;

        public SaveData()
        {
            Facts = new FactDictionary();
        }

        public void Save<T>(string factname, T value)
        {
            try
            {
                T val = value;
                Load<T>(factname, default);
                // If the load is unsuccessful, that means that the value is of a different type and a casting error is thrown
                Facts[factname] = value;
            }
            catch
            {
                App.Instance.Logger.Log(LoggingGroupID.APP, $"Unable to store fact {factname}. Check if this fact is already being stored under a different type");
            }
        }

        public T Load<T>(string factName, T fallback)
        {
            try
            {
                if (Facts.ContainsKey(factName))
                {
                    Debug.Log((T)Facts[factName]);
                    return (T)Facts[factName];
                }
                else
                    return fallback;
            }
            catch (System.Exception error)
            {
                App.Instance.Logger.Log(LoggingGroupID.APP, error, LogType.Warning);
                return fallback;
            }
        }
    }
}
