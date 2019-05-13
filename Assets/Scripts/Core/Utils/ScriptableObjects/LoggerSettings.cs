using System.Collections.Generic;
using UnityEngine;
using LogType = Core.Utils.Logger.LogType;

namespace Core.Utils
{
    [CreateAssetMenu(menuName = "Core/Logger", fileName = "LogSettings")]
    public class LoggerSettings : ScriptableObject
    {
        [SerializeField] private List<LogType> acceptableLogLevels;

        public List<LogType> AcceptableLogLevels => acceptableLogLevels;
    }
}