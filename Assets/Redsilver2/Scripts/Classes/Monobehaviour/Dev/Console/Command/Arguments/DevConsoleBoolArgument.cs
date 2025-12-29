using UnityEngine;
namespace RedSilver2.Framework.Dev
{
    public abstract partial class DevConsole : MonoBehaviour
    {
        public sealed class DevConsoleBoolArgument : DevConsoleArgument
        {
            public DevConsoleBoolArgument(string details) : base(details) {

            }

            public sealed override async Awaitable<string> GetPreview() {
                return details + " [Boolean]";
            }

            public sealed override async Awaitable<bool> IsValid(string argument)
            {

                if (!string.IsNullOrEmpty(argument))  {
                    argument = argument.ToLower();
                    Debug.Log(argument);

                    if (argument.Equals("t") || argument.Equals("true")) 
                        return true;

                    if (argument.Equals("f") || argument.Equals("false"))
                       return true;
                }

                Debug.Log(argument + " not valid");
                return false;
            }

        }
    }
}
