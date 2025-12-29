using UnityEngine;
namespace RedSilver2.Framework.Dev
{
    public abstract partial class DevConsole : MonoBehaviour
    {
        public sealed class DevConsoleStringArgument : DevConsoleArgument
        {
            private readonly string[] validValues;
           
            private const char SEPERATOR      = '|';
            private const char LEFT_BRACKET   = '(';
            private const char RIGHT_BRACKET  = ')';

            public DevConsoleStringArgument(string details) : base(details)
            {
                this.validValues = null;
            }

            public DevConsoleStringArgument(string details, string[] validValues) : base(details)
            {
                this.validValues = validValues;
            }

            public sealed override async Awaitable<string> GetPreview()
            {
                return $"{details} {await GetPreview(validValues)} [String]";
            }

            private async Awaitable<string> GetPreview(string[] validValues)
            {
                string results = string.Empty;
                if (validValues == null) return results;

                await Awaitable.BackgroundThreadAsync();
                results += $"{LEFT_BRACKET}";

                for (int i = 0; i < validValues.Length; i++)
                    results += validValues[i] +  (i != validValues.Length - 1 ? $" {SEPERATOR} " : string.Empty  );

                await Awaitable.MainThreadAsync();
                return $"{results}{RIGHT_BRACKET}";
            }

            public sealed override async Awaitable<bool> IsValid(string argument)
            {
                if(validValues != null){
                    await Awaitable.BackgroundThreadAsync();

                    for (int i = 0; i < validValues.Length; i++)
                        if (argument.ToLower().Equals(validValues[i].ToLower())) {
                            await Awaitable.MainThreadAsync();
                            return true;
                        }

                    await Awaitable.MainThreadAsync();
                    return false;
                }

                return true;
            }
        }
    }
}
