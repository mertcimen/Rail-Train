using UnityEngine;
using System;
using System.Collections.Generic;

public class UnityLogSaver : MonoBehaviour
{
    struct Log
    {
        public string message;
        public string stackTrace;
        public LogType type;
    }

    // List<Log> logs = new List<Log>();

    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
        // Application.RegisterLogCallback(null);
    }


    void HandleLog(string message, string stackTrace, LogType type)
    {
        Log log = new Log()
        {
            message = message,
            stackTrace = stackTrace,
            type = type,
        };

        SaveLog(log);
    }

    void SaveLog(Log log)
    {
        FileLogger.WriteLog(GetTimeAsString(Time.time) + "\t[" + log.type.ToString() + "] " + log.message);
        if (log.stackTrace != "")
            FileLogger.WriteLog("\t\t\t\t\t" + log.stackTrace);
    }

    private string GetTimeAsString(float time)
    {
        time = Math.Abs(time);
        TimeSpan timeSpan = TimeSpan.FromSeconds(time);
        string minutes = timeSpan.Minutes.ToString("00");
        string seconds = timeSpan.Seconds.ToString("00");
        string milliseconds = timeSpan.Milliseconds.ToString("00");

        return string.Format("{0}:{1}:{2}", minutes, seconds, milliseconds);
    }
}