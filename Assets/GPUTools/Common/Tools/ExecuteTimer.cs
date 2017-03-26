using System;
using UnityEngine;

namespace GPUTools.Common.Tools
{
    public class ExecuteTimer
    {
        public static DateTime StartTime;

        public static void Start()
        {
            StartTime = DateTime.Now;;
        }

        public static double TotalMiliseconds()
        {
            return (DateTime.Now - StartTime).TotalMilliseconds;
        }

        public static void Log()
        {
           Debug.Log("Total Miliseconds: " + TotalMiliseconds());
        }
    }
}
