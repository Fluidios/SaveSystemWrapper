using UnityEngine;

namespace Game.SaveSystem.Core
{
    public static class SMLogger
    {
        public static void Log(string message)
        {
            Debug.Log(message);
        }
        public static void LogWarning(string message)
        {
            Debug.LogWarning(message);
        }
        public static void LogError(string message)
        {
            Debug.LogError(message);
        }
    }
}
