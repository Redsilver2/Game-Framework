using UnityEngine;

namespace RedSilver2.Framework.Dev
{
    public abstract partial class DevConsole : MonoBehaviour
    {
        public sealed class LogErrorCommand : LogCommand
        {
            protected sealed override void LogMessage(string message, bool useUnityLog)
            {
                LogError(message, useUnityLog);
            }

            protected sealed override void SetPrefix(ref string prefix)
            {
                prefix = "Log Error";
            }
        }
    }
}
