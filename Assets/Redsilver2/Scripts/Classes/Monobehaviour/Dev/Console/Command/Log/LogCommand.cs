using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace RedSilver2.Framework.Dev
{
    public abstract partial class DevConsole : MonoBehaviour
    {
        public abstract class LogCommand : DevConsoleCommand
        {
            protected override void SetActions(ref List<DevConsoleCommandAction> actions)
            {
                if (actions == null) actions = new List<DevConsoleCommandAction>();

                actions.Add(new DevConsoleCommandAction(new DevConsoleArgument[] {
                   new DevConsoleStringArgument("message"),
                },
                LogAction01(),
                false));


                actions.Add(new DevConsoleCommandAction(new DevConsoleArgument[] {
                   new DevConsoleBoolArgument("useUnityLog"),
                   new DevConsoleStringArgument("message"),
                },
                LogAction02(),
                false));

            }

            private UnityAction<string[]> LogAction01()
            {
                return args => {
                    try {
                        LogMessage(GetMessage(0, args), false);
                    }
                    catch {
                        DevConsole.LogError("....");
                    }
                };
            }

            private UnityAction<string[]> LogAction02()
            {
                return args => {
                    try {
                        bool useUnityLog = false;
                        args[0] = args[0].ToLower(); 

                        if (args[0].Equals("t") || args[0].Equals("true"))       useUnityLog = true;
                        else if (args[0].Equals("f") || args[0].Equals("false")) useUnityLog = false;

                        LogMessage(GetMessage(1, args), useUnityLog);
                    }
                    catch {
                        DevConsole.LogError("....");
                    }
                };
            }

            private string GetMessage(int startingIndex, string[] args)
            {
                string message = string.Empty;
                if (args == null || args.Length == 0) return string.Empty;

                for (int i = startingIndex; i < args.Length; i++)
                    message += args[i] + " ";

                return message;
            }

            protected abstract void LogMessage(string message, bool useUnityLog);
        }
    }
}
