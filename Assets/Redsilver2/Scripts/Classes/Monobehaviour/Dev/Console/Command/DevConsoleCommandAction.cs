using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Dev
{
    public abstract partial class DevConsole : MonoBehaviour
    {
        public sealed class DevConsoleCommandAction
        {
            private readonly DevConsoleArgument[]  arguments;
            private readonly UnityAction<string[]> onExecuted;
            private readonly bool exactNumberOfArgs;

            public int ArgumentCount {
                get {
                    if (arguments == null) return 0;
                    return arguments.Length; 
                }
            }




            public DevConsoleCommandAction(UnityAction<string[]> onExecuted, bool exactNumberOfArgs)
            {
                this.arguments = new DevConsoleArgument[0];
                this.onExecuted = onExecuted;
                this.exactNumberOfArgs = exactNumberOfArgs;
            }

            public DevConsoleCommandAction(DevConsoleArgument[] arguments, UnityAction<string[]> onExecuted, bool exactNumberOfArgs) {
                this.arguments         = arguments;
                this.onExecuted        = onExecuted;
                this.exactNumberOfArgs = exactNumberOfArgs;
            }

            public async void Execute(string[] args) 
            {
                if(await IsValid(args))
                   if(onExecuted != null)
                        onExecuted.Invoke(args);    
            }

            public async Awaitable<string> GetPreview()
            {
                string result = string.Empty;
                if (arguments == null) return result;

                await Awaitable.BackgroundThreadAsync();

                foreach(DevConsoleArgument argument in arguments)
                    result += $"{await argument.GetPreview()} ";


                await Awaitable.MainThreadAsync();
                return result;
            }

            public async Awaitable<bool> IsValid(string[] args)
            {
                if (args == null || arguments == null)
                    return false;

                if(exactNumberOfArgs && args.Length != arguments.Length)
                    return false;

                await Awaitable.BackgroundThreadAsync();

                for (int i = 0; i < arguments.Length; i++)
                    if (!await arguments[i].IsValid(args[i])){
                        await Awaitable.MainThreadAsync();
                        return false;
                    }

                await Awaitable.MainThreadAsync();
                return true;
            }

            public bool IsPreviewAllowed(string[] args)
            {
                 if (args == null || arguments == null) return false;
                 return true;
            }
        }
    }
}
