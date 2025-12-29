using System.Collections.Generic;
using UnityEngine;
namespace RedSilver2.Framework.Dev
{
    public abstract partial class DevConsole : MonoBehaviour
    {
        public sealed class LogWarningCommand : LogCommand
        {
            protected sealed override void SetPrefix(ref string prefix)
            {
                prefix = "Log Warning";
            }


            protected sealed override void LogMessage(string message, bool useUnityLog) {
                LogWarning(message, useUnityLog);
            }

        }
    }
}
