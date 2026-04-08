using RedSilver2.Framework.Inputs.Settings;
using System.Linq;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.States.Configurations
{
    [CreateAssetMenu(fileName = "New Run State Settings", menuName = "States/Settings/Movement/Run")]
    public class RunStateSettings : MovementStateSettings
    {
        public PressInputSettings PressRunInputSettings;
        public HoldInputSettings  HoldRunInputSettings;

        protected sealed override MovementStateConfiguration GetMovementStateConfiguration(MovementStateMachine stateMachine) {
            return new RunStateConfiguration(stateMachine, this);
        }

        public RunStateConfiguration GetConfiguration(MovementStateMachine machine) {
            return GetBaseConfiguration(machine) as RunStateConfiguration;
        }

        public static RunStateSettings GetSettings(MovementStateMachine machine)
        {
            StateSettings[] settings = GetBaseSettings(machine);
            if (settings.Length == 0) return null;

            var results = settings.Where(x => x is RunStateSettings);
            if (results.Count() > 0) return results.First() as RunStateSettings;

            return null;
        }
    }
}
