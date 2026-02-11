using UnityEngine;

namespace RedSilver2.Framework.StateMachines.States {
    public abstract class MovementSoundExtension : MovementStateExtension
    {
        protected MovementSoundModule soundModule;

        protected override void Awake()
        {
            base.Awake();
            soundModule = GetComponent<MovementSoundModule>();
        }

        protected override void Start()
        {
            base.Start();
            stateMachine?.AddOnStateModuleAddedListener(OnStateModuleAdded);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            if(didStart) stateMachine?.RemoveOnStateModuleAddedListener(OnStateModuleAdded);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            if(didStart) stateMachine?.AddOnStateModuleAddedListener(OnStateModuleAdded);
        }

        private void OnStateModuleAdded(StateModule module)
        {
            if(module is MovementSoundModule) soundModule = (module as MovementSoundModule);
        }


    }
}
