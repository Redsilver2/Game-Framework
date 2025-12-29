using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Windows;
using static UnityEngine.Rendering.GPUSort;

namespace RedSilver2.Framework.Dev
{
    public abstract partial class DevConsole : MonoBehaviour
    {
        public abstract class DevConsoleCommand
        {
            public  readonly string                          prefix; 
            private readonly List<DevConsoleCommandAction>   actions;

            public DevConsoleCommand() {
                actions = new List<DevConsoleCommandAction>();
                SetPrefix (ref prefix);
                SetActions(ref actions);

                DevConsole.commands.Add(this);
            }

            protected abstract void SetPrefix(ref string prefix);
            protected abstract void SetActions(ref List<DevConsoleCommandAction> actions);

            public async Awaitable<bool> IsValid(string input)
            {
                string[] args, prefixs;
                if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(prefix)) return false;

                await Awaitable.BackgroundThreadAsync();

                args    = input .ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                prefixs = prefix.ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries);

                if (args == null || prefixs == null || args.Length < prefixs.Length)
{
                    await Awaitable.MainThreadAsync();
                    return false;
                }  

                for (int i = 0; i < prefixs.Length; i++)
                    if (!prefixs[i].Equals(args[i])) return false;

                await Awaitable.MainThreadAsync();
                return true;
            }

            public async Awaitable<string[]> GetActionPreviews() {

                List<string> results = new List<string>();
                if (actions == null || actions.Count == 0) return results.ToArray();

                await Awaitable.BackgroundThreadAsync();

                foreach (DevConsoleCommandAction action in actions)
                    results.Add($"{prefix} {await action.GetPreview()}");

                await Awaitable.MainThreadAsync();
                return results.ToArray();
            }

            public async Awaitable<string[]> GetActionPreviews(string input)
            {
                List<string> results = new List<string>();
                if (actions == null || actions.Count == 0 || string.IsNullOrEmpty(input) || !await IsValid(input)) return results.ToArray();

                await Awaitable.BackgroundThreadAsync();

                foreach (DevConsoleCommandAction action in actions)
                      results.Add($"{prefix} {await action.GetPreview()}");

                await Awaitable.MainThreadAsync();
                return results.ToArray();
            }


            private async Awaitable<string[]> GetFormattedArguments(string input)
            {
                if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(prefix)) return default;
                await Awaitable.BackgroundThreadAsync();

                string[] args                   = input .Split(' ', StringSplitOptions.RemoveEmptyEntries);
                string[] prefixs                = prefix.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                int   [] invalidArgumentIndexes = await GetInvalidArgumentIndexes(prefixs, args);

                await Awaitable.MainThreadAsync();
                return await GetFormattedArguments(invalidArgumentIndexes, args, prefixs.Length);
            }

            private async Awaitable<int[]> GetInvalidArgumentIndexes(string[] prefixs, string[] args)
            {
                List<int> results = new List<int>();
              
                if (prefixs == null || args == null || prefixs.Length == 0 || args.Length == 0)
                    return results.ToArray();

                await Awaitable.BackgroundThreadAsync();

                for (int i = 0; i < prefixs.Length; i++)
                    if (prefixs[i].ToLower().Equals(args[i].ToLower()))
                        results.Add(i);

                await Awaitable.MainThreadAsync();
                return results.ToArray();
            }

            private async Awaitable<string[]> GetFormattedArguments(int[] registeredIndexes, string[] args, int prefixCount)
            {
                List<string> results = new List<string>();

                if(registeredIndexes == null || args == null || args.Length == 0 || registeredIndexes.Length == 0 || prefixCount == 0) 
                    return results.ToArray();

                await Awaitable.BackgroundThreadAsync();

                if (registeredIndexes.Length == prefixCount) {
                    for (int i = 0; i < args.Length; i++)
                        if(!registeredIndexes.Contains(i))
                            results.Add(args[i]);
                }

                await Awaitable.MainThreadAsync();
                return results.ToArray();   
            }

            public async void Execute(string input)
            {
                string[] args = await GetFormattedArguments(input);
                var results   = (await GetValidActions(args)).OrderBy(x => x.ArgumentCount).Reverse();
                if (results.Count() > 0) results.First().Execute(args);
            }

            private async Awaitable<DevConsoleCommandAction[]> GetValidActions(string[] args)
            {
                List<DevConsoleCommandAction> results = new List<DevConsoleCommandAction>();
                if (actions == null || actions.Count == 0) return results.ToArray();

                await Awaitable.BackgroundThreadAsync();

                foreach (DevConsoleCommandAction action in actions.Where(x => x != null))
                    if (await action.IsValid(args))
                        results.Add(action);

                await Awaitable.MainThreadAsync();
                return results.ToArray();                 
            }
        }
    }
}
