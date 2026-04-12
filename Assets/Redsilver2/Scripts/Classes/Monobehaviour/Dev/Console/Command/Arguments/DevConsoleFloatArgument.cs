using UnityEngine;

namespace RedSilver2.Framework.Dev
{
    public abstract partial class DevConsole : MonoBehaviour
    {
        public sealed class DevConsoleFloatArgument : DevConsoleArgument
        {
            public DevConsoleFloatArgument(string details) : base(details) {

            }

            public sealed override async Awaitable<string> GetPreview()
            {
                return details + " [Float]";
            }

            public sealed override async Awaitable<bool> IsValid(string argument)
            {
                if (!string.IsNullOrEmpty(argument)) return float.TryParse(argument, out var value);
                return false;
            }
        }
    }
}
