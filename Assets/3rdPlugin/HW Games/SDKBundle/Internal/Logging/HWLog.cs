using System;
using UnityEngine;

namespace HWGames.Bundles.Internal
{
    public static class HWLog
    {
        private static HWLogLevel _logLevel;
        private const string TAG = "SDKBundle";
        
        public static void Initialize(HWLogLevel level)
        {
            _logLevel = level;
            EnableLogs();
        }

        public static void Log(string tag, string message)
        {
            if (!Application.isEditor || _logLevel >= HWLogLevel.DEBUG)
                Debug.Log(Format(tag, message));
        }

        public static void LogE(string tag, string message)
        {
            if (!Application.isEditor || _logLevel >= HWLogLevel.ERROR)
                Debug.LogError(Format(tag, message));
        }

        public static void LogW(string tag, string message)
        {
            if (!Application.isEditor || _logLevel >= HWLogLevel.WARNING)
                Debug.LogWarning(Format(tag, message));
        }
        
        private static string Format(string tag, string message) => $"{DateTime.Now} - {TAG}/{tag}: {message}";

        // Separate Log enable/disable separate from LogLevel 
        public static void DisableLogs()
        {
            Debug.unityLogger.logEnabled = false;
        }

        private static void EnableLogs()
        {
            Debug.unityLogger.logEnabled = true;
        }

        public static bool IsLogsEnabled() => Debug.unityLogger.logEnabled;
    }

    public enum HWLogLevel
    {
        ERROR=0,
        WARNING=1,
        DEBUG=2
    }
}