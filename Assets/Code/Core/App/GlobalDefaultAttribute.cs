using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CurlyCore
{
    public class GlobalDefaultAttribute : PropertyAttribute
    {
        public bool UseDefault;

        public GlobalDefaultAttribute(bool useDefault = true)
        {
            UseDefault = useDefault;
        }
    }
}
