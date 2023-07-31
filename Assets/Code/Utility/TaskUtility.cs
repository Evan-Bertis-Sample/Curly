using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;

namespace Utility
{
    public class TaskUtility
    {
        public static async Task WaitUntil(Func<bool> condition)
        {
            while (condition() == false) await Task.Yield();
        }

    }
}