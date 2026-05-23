using UnityEngine;

namespace RedSilver2.Framework.Interactions.Actions.Setups
{
    public class PlayAudioSetup : AudioInteractionActionSetup
    {
        [SerializeField] private PlayAudio action;
        private AudioInteractionModule module;

        protected sealed override void OnDisable()
        {
            Disable();
        }

        protected sealed override void OnEnable()
        {
            Enable();
        }

        protected sealed override void SetInteractionModule(AudioInteractionModule module)
        {
            this.module = module;
            Enable();
        }

        private void OnPlay(AudioClip clip)
        {
            if(clip != null) action?.Disable(module);
        }

        private void OnStop() {
            action?.Enable(module);
        }

        private void Enable() {
            action?.Add(module);
            module?.AddOnPlayListener(OnPlay);

            module?.AddOnStopListener(OnStop);
            action?.Enable(module);

            if (module == null || !module.IsPlaying()) return;
            action?.Disable(module);
        }

        private void Disable()
        {
            action?.Remove(module);
            module?.RemoveOnPlayListener(OnPlay);
            module?.RemoveOnStopListener(OnStop);
        }
    }
}
