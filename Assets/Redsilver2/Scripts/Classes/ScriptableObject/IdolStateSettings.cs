using UnityEngine;

namespace RedSilver2.Framework.StateMachines.States.Configurations
{
    [CreateAssetMenu(fileName = "New Idol State Settings", menuName = "States/Settings/Movement/Idol")]
    public class IdolStateSettings : MovementStateSettings
    {
        protected sealed override MovementStateConfiguration GetMovementStateConfiguration(MovementStateMachine stateMachine) {     
            return new IdolStateConfiguration(stateMachine);
        }

        public IdolStateConfiguration GetConfiguration(MovementStateMachine stateMachine) {
            return GetBaseConfiguration(stateMachine) as IdolStateConfiguration;
        }
    }
}
