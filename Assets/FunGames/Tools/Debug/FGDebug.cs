using UnityEngine;

namespace FunGames.Tools.Debugging
{
    public static class FGDebug 
    {
        private static void Log(string message, string color)
        {
            Debug.Log("<color=" + color + ">" + message + "</color>");
        }
        
        public static void Log(string message, Color color)
        {
            Debug.Log("<color=#" + ColorUtils.FromColor(color) + ">" + message + "</color>");
        }

        public static void AssertionLog(bool result, string message)
        {
            if (result)
            {
                Debug.Log($"<color=green>{message + " : OK!"}</color>\n");
            }
            else
            {
                Debug.LogError(message + " : NOK!");
            }
        }
    }
}