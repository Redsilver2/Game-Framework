using UnityEngine;
using UnityEngine.UIElements;

namespace RedSilver2.Framework.Player
{
    public partial class PlayerStateMachine
    {
        public class HeadBobExtension : PlayerExtension
        {
            private IsOnGround isOnGround;
            private IsMoving   isMoving;

            private float positionXProgress;
            private float positionYProgress;

            private float currentPositionX;
            private float currentPositionY;

            private bool canHeabob = true;
            private bool isHeadbobEnabled = true;

            private Transform transform;
            private Vector2   starterPosition;
            private Vector2   min;
            private Vector2   max;

            public const string EXTENSION_NAME = "Headbob Extension";

            public HeadBobExtension(PlayerStateMachine owner, Transform transform, Vector2 min, Vector2 max) : base(owner)
            {
                this.transform = transform;
                if(transform != null) this.starterPosition = transform.localPosition;

                this.min = min;
                this.max = max;


                currentPositionX = starterPosition.x;
                currentPositionY = starterPosition.y;
            }

            public HeadBobExtension(PlayerStateMachine owner, Transform transform, Vector2 startPosition, Vector2 min, Vector2 max) : base(owner)
            {
                if(owner != null) transform = owner.owner.transform;
                this.starterPosition = startPosition;

                this.min = min;
                this.max = max;

                currentPositionX = starterPosition.x;
                currentPositionY = starterPosition.y;
            }

            protected sealed override void OnStateAdded(PlayerState state)
            {
                base.OnStateAdded(state);
                AddEvents(state);
            }

            protected sealed override void OnStateRemoved(PlayerState state)
            {
                base.OnStateRemoved(state);
                RemoveEvents(state);
            }

            private void OnUpdate()
            {
                if (isHeadbobEnabled && canHeabob)
                    UpdateHeadbob();
                else
                    ResetHeadbob();
            }

            private void OnLateUpdate()
            {
                if (transform != null)
                {
                    transform.localPosition = transform.right * currentPositionX + transform.up * currentPositionY;
                }
            }

            private void UpdateHeadbob()
            {
                float positionX, positionY;

                positionXProgress = Mathf.Abs(Mathf.Sin(Time.time * 8f));
                positionYProgress = Mathf.Abs(Mathf.Cos(Time.time * 8f));

                positionX = Mathf.Lerp(min.x, max.x, positionXProgress);
                positionY = Mathf.Lerp(min.y, max.y, positionYProgress);

                currentPositionX = Mathf.Lerp(currentPositionY, positionX, Time.deltaTime);
                currentPositionY = Mathf.Lerp(currentPositionX, positionY, Time.deltaTime);
            }

            private void ResetHeadbob()
            {
                positionXProgress = Mathf.Lerp(positionXProgress, 0f, Time.deltaTime);
                positionYProgress = Mathf.Lerp(positionYProgress, 0f, Time.deltaTime);

                currentPositionX = Mathf.Lerp(currentPositionY, 0f, Time.deltaTime);
                currentPositionY = Mathf.Lerp(currentPositionX, 0f, Time.deltaTime);

            }

            private void AddEvents(PlayerState state)
            {
                AddUpdateEvents();
                AddOnStateEnterEvent(state);
            }

            private void RemoveEvents(PlayerState state)
            {
                RemoveUpdateEvents();
                RemoveOnStateEnterEvent(state);
            }

            protected override void OnDisable()
            {
                base.OnDisable();
            }

            private void RemoveUpdateEvents()
            {
                if (owner == null) return;

                if (owner.GetStates(MoveState.RequiredInputStates).Length == 0)
                {
                    owner.RemoveOnUpdateListener(OnUpdate);
                    owner.RemoveOnLateUpdateListener(OnLateUpdate);
                    transform.localPosition = starterPosition;
                }
            }

            private void RemoveOnStateEnterEvent(PlayerState state)
            {
                if (state != null)
                {
                    if (state is WalkState || state is RunState || state is CrouchState)
                        state.RemoveOnEnterStateListener(OnHeadbobValideStateEnter);
                    else
                        state.RemoveOnEnterStateListener(OnHeadBobInvalideStateEnter);
                }
            }

            private void AddUpdateEvents()
            {
                if (owner == null) return;

                if (owner.GetStates(MoveState.RequiredInputStates).Length == 1)
                {
                    owner.AddOnUpdateListener(OnUpdate);
                    owner.AddOnLateUpdateListener(OnLateUpdate);
                }
            }

            private void AddOnStateEnterEvent(PlayerState state)
            {
                if (state != null)
                {
                    if (state is WalkState || state is RunState || state is CrouchState)
                        state.AddOnEnterStateListener(OnHeadbobValideStateEnter);
                    else
                        state.AddOnEnterStateListener(OnHeadBobInvalideStateEnter);
                }
            }

            private void OnHeadbobValideStateEnter()
            {
                canHeabob = true;
            }

            private void OnHeadBobInvalideStateEnter()
            {
                canHeabob = false;
            }

            protected override string GetExtensionName() => EXTENSION_NAME;
        }

    }
}
