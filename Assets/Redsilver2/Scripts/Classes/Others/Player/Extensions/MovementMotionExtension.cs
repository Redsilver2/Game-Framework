using RedSilver2.Framework.Inputs;
using UnityEngine;
namespace RedSilver2.Framework.Player
{
    public partial class PlayerStateMachine
    {
        public abstract class MovementMotionExtension : PlayerExtension
        {

            private bool canApplyMotion;
            private bool isMotionEnabled;

            protected readonly Transform transform;
            protected readonly Vector2Input input;

            protected MovementMotionExtension(PlayerStateMachine owner, Transform transform) : base(owner)
            {
                canApplyMotion  = false;
                isMotionEnabled = true;
              
                this.transform  = transform;
                this.input      = GetInput();
            }

            private void OnUpdate()
            {
                if (canApplyMotion && isMotionEnabled && input != null)
                    UpdateMotion(input.Value);
                else
                    ResetMotion();
            }

            protected sealed override void OnDisable()
            {
                base.OnDisable();

                isMotionEnabled = false;
                canApplyMotion  = false; 


                if (owner != null)
                {
                    owner.RemoveOnUpdateListener(OnUpdate);
                    owner.RemoveOnLateUpdateListener(OnLateUpdate);
                }
            }

            protected sealed override void OnEnable()
            {
                base.OnEnable(); 
                isMotionEnabled = true; 

                if (owner != null)
                {
                    PlayerState[] states = owner.GetStates();

                    canApplyMotion = IsCompatibleState(states == null || states.Length == 0 ? null : states[0]); // <--- Change this depending current state
                    owner.AddOnUpdateListener(OnUpdate);
                    owner.AddOnLateUpdateListener(OnLateUpdate);
                }
            }

            protected sealed override void OnStateEnter(PlayerState state)
            {
                canApplyMotion = IsCompatibleState(state);
                Debug.Log("Can apply motion? : "  + IsCompatibleState(state));  
            }


            protected abstract void OnLateUpdate();
            protected abstract void UpdateMotion(Vector2 inputMotion);
            protected abstract void ResetMotion();
            protected abstract Vector2Input GetInput();
        }
    }
}
