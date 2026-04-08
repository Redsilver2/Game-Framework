using System.Linq;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.States.Configurations
{
    [CreateAssetMenu(fileName = "New Walk State Settings", menuName = "States/Settings/Movement/Walk")]
    public sealed class WalkStateSettings : MovementStateSettings
    {
        protected override MovementStateConfiguration GetMovementStateConfiguration(MovementStateMachine stateMachine)  {
            return new WalkStateConfiguration(stateMachine);
        }

        public WalkStateConfiguration GetConfiguration(MovementStateMachine stateMachine)
        {
            return GetBaseConfiguration(stateMachine) as WalkStateConfiguration;    
        }

        public static WalkStateSettings GetSettings(MovementStateMachine stateMachine)
        {
            StateSettings[] settings = GetBaseSettings(stateMachine);
            if (settings.Length == 0) return null;

            var results = settings.Where(x => x is WalkStateSettings);
            if (results.Count() > 0) return results.First() as WalkStateSettings;

            return null;
        }
    }
}
