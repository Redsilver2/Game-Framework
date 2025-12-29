using UnityEngine;

namespace RedSilver2.Framework.Dev
{
    public abstract partial class DevConsole : MonoBehaviour
    {
        public abstract class DevConsoleArgument
        {
            protected readonly string details;
            
            protected DevConsoleArgument(string details) {
                this.details = details;
            }

            public abstract Awaitable<bool> IsValid(string argument);
            public abstract Awaitable<string> GetPreview();
        }
    }
}
