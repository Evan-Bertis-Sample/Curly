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

        public void Save<T>(string factname, T value)
        {
            try
            {
                T val = value;
                Load<T>(factname, ref val, default);
                // If the load is unsuccessful, that means that the value is of a different type and a casting error is thrown
                Facts[factname] = value;
            }
            catch
            {
                App.Instance.Logger.Log(LoggingGroupID.APP, $"Unable to store fact {factname}. Check if this fact is already being stored under a different type");
            }
        }

        public void Load<T>(string factName, ref T value, T fallback)
        {
            try
            {
                if (Facts.ContainsKey(factName))
                    value = (T)Facts[factName];
                else
                    value = fallback;
            }
            catch (System.Exception error)
            {
                App.Instance.Logger.Log(LoggingGroupID.APP, error, LogType.Warning);
                value = fallback;
            }
        }
    }
}
