using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Dev
{
    public abstract partial class DevConsole : MonoBehaviour
    {
        public sealed class TeleportCommand : DevConsoleCommand
        {
            private const string PREFIX = "Teleport";

            protected sealed override void SetPrefix(ref string prefix){
                prefix = PREFIX;
            }

            protected sealed override void SetActions(ref List<DevConsoleCommandAction> actions)
            {
                if (actions == null) actions = new List<DevConsoleCommandAction>();

                actions.Add(new DevConsoleCommandAction(
                    GetTeleportToDestinationArguments01(),
                    GetTeleportToDestinationAction01(),
                    true
                ));

                actions.Add(new DevConsoleCommandAction(
                    GetTeleportToXYZArguments01(),
                    GetTeleportXYZAction01(),
                    true
                ));

                actions.Add(new DevConsoleCommandAction(
                    GetTeleportToXYZArguments02(),
                    GetTeleportXYZAction02(),
                    true
                ));
            }

            private DevConsoleArgument[] GetBaseTeleportToArguments()
            {
                return new DevConsoleArgument[] {
                     new DevConsoleStringArgument("targetName"),
                };
            }

            private DevConsoleArgument[] GetTeleportToDestinationArguments01()
            {
                List<DevConsoleArgument> arguments = GetBaseTeleportToArguments().ToList();
                arguments.Add(new DevConsoleStringArgument("destinationName"));
                return arguments.ToArray();
            }

            private DevConsoleArgument[] GetTeleportToXYZArguments01()
            {
                List<DevConsoleArgument> arguments = GetBaseTeleportToArguments().ToList();
                arguments.Add(new DevConsoleFloatArgument("X"));
                arguments.Add(new DevConsoleFloatArgument("Y"));
                arguments.Add(new DevConsoleFloatArgument("Z"));
                return arguments.ToArray();
            }

            private DevConsoleArgument[] GetTeleportToXYZArguments02() {
                List<DevConsoleArgument> arguments = GetTeleportToXYZArguments01().ToList();
                arguments.Add(new DevConsoleStringArgument("teleportationType", new string[] { "Global", "Local" }));
                return arguments.ToArray();
            }

            private UnityAction<string[]> GetTeleportXYZAction01()
            {
                return args =>
                {
                    try {
                        TeleportToVector3(GameObject.Find(args[0]),
                                          new Vector3(float.Parse(args[1]), float.Parse(args[2]), float.Parse(args[3])),
                                          false);
                    }
                    catch{
                        DevConsole.LogError("One or multiple parameter(s) are  ");
                    }
                };
            }
            private UnityAction<string[]> GetTeleportXYZAction02()
            {
                return args =>
                {
                    try
                    {
                        bool isLocalPositionChange = false;

                        args[4] = args[4].ToLower();

                        if      (args[4].Equals("global")) { isLocalPositionChange = false; }
                        else if (args[4].Equals("local"))  { isLocalPositionChange = true; }
                        else                               { return; }

                        TeleportToVector3(GameObject.Find(args[0]),
                                          new Vector3(float.Parse(args[1]), float.Parse(args[2]), float.Parse(args[3])),
                                          isLocalPositionChange);
                    }
                    catch
                    {
                        DevConsole.LogError("One or multiple parameter(s) are  ");
                    }
                };
            }

            private UnityAction<string[]> GetTeleportToDestinationAction01()
            {
                return args =>
                {
                    try {
                        DevConsole.Log($"Teleported {args[0]} to {args[1]}");
                        TeleportToGameObject(GameObject.Find(args[0]), GameObject.Find(args[1]));
                    }
                    catch
                    {
                        DevConsole.LogError("One or multiple parameter(s) are  ");
                    }
                };
            }




            private void TeleportToGameObject(GameObject target, GameObject destination)
            {
                if (target == null || destination == null) return;
                DevConsole.Log($"Teleported {target.name} to {destination.name}");
                TeleportToVector3(target, destination.transform.position, false);
            }

            private void TeleportToVector3(GameObject target, Vector3 vector, bool useLocalPosition)
            {
                if(target == null) return;
                if (useLocalPosition) target.transform.localPosition = vector;
                else                  target.transform.position      = vector;
            }
        }
    }
}
