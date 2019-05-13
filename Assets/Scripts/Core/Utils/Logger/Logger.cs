using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Utils.Logger
{
    public class Logger
    {
        private Dictionary<LogLevel, Action<string, LogType>> log;
        private LoggerSettings loggerSettings;
        
        public Logger(LoggerSettings settings)
        {
            loggerSettings = settings;
            
            log = new Dictionary<LogLevel, Action<string, LogType>>()
            {
                {
                    LogLevel.Warning, LogWarning
                },
                {
                    LogLevel.Error, LogError
                },
                {
                    LogLevel.Normal, LogNormal
                },
                {
                    LogLevel.File, LogFile
                }
            };
        }

        public void Log(string msg, LogType type = LogType.Other , LogLevel level = LogLevel.Normal)
        {
            if (!loggerSettings.AcceptableLogLevels.Contains(type))
            {
                return;
            }

            log[level].InvokeSafe(msg, type);
        }

        private void LogNormal(string e, LogType t)
        {
            Debug.Log($"{t} \n {e}");
        }
        
        private void LogWarning(string e, LogType t)
        {
            Debug.LogWarning($"{t} \n {e}");
        }
        
        private void LogError(string e, LogType t)
        {
            Debug.LogError($"{t} \n {e}");
        }
        
        private void LogFile(string e, LogType t)
        {
            
        }
    }
}