using UnityEngine;

namespace CurlyCore.Debugging
{
    [System.Serializable]
    public struct LoggingGroup
    {
        public string GroupName;
        public Color LoggingColor;

        public LoggingGroup(string groupName, Color loggingColor)
        {
            GroupName = groupName;
            LoggingColor = loggingColor;
        }
    }
}

