using System;
using UnityEngine;
using Object = UnityEngine.Object;

public static class LogManager
{
    private static Logger myLogger;

    private static bool _isLoggingActive = true;

    public static void Initialize(bool status)
    {
        myLogger = new Logger(Debug.unityLogger.logHandler);
        _isLoggingActive = status;
    }

    public static void LogFormat(LogType logType, Object context, string format, params object[] args)
    {
        if (!_isLoggingActive)
            return;

        myLogger.LogFormat(logType, context, format, args);
    }

    public static void LogException(Exception exception, Object context)
    {
        if (!_isLoggingActive)
            return;

        myLogger.LogException(exception, context);
    }

    public static void Log(LogType logType, object message)
    {
        if (!_isLoggingActive)
            return;

        myLogger.Log(logType, message);
    }

    public static void Log(LogType logType, object message, Object context)
    {
        if (!_isLoggingActive)
            return;

        myLogger.Log(logType, message, context);
    }

    public static void Log(LogType logType, string tag, object message)
    {
        if (!_isLoggingActive)
            return;

        myLogger.Log(logType, tag, message);
    }

    public static void Log(LogType logType, string tag, object message, Object context)
    {
        if (!_isLoggingActive)
            return;

        myLogger.Log(logType, tag, message, context);
    }

    public static void Log(object message)
    {
        if (!_isLoggingActive)
            return;

        myLogger.Log(message);
    }

    public static void Log(string tag, object message)
    {
        if (!_isLoggingActive)
            return;

        myLogger.Log(tag, message);
    }

    public static void Log(string tag, object message, Object context)
    {
        if (!_isLoggingActive)
            return;

        myLogger.Log(tag, message, context);
    }

    public static void LogWarning(string tag, object message)
    {
        if (!_isLoggingActive)
            return;

        myLogger.LogWarning(tag, message);
    }

    public static void LogWarning(string tag, object message, Object context)
    {
        if (!_isLoggingActive)
            return;

        myLogger.LogWarning(tag, message, context);
    }

    public static void LogError(string tag, object message)
    {
        if (!_isLoggingActive)
            return;

        myLogger.LogError(tag, message);
    }

    public static void LogError(string tag, object message, Object context)
    {
        if (!_isLoggingActive)
            return;

        myLogger.LogError(tag, message, context);
    }

    public static void LogFormat(LogType logType, string format, params object[] args)
    {
        if (!_isLoggingActive)
            return;

        myLogger.LogFormat(logType, format, args);
    }

    public static void LogException(Exception exception)
    {
        if (!_isLoggingActive)
            return;

        myLogger.LogException(exception);
    }
}