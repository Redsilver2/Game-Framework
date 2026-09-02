using RedSilver2.Framework.StateMachines.States;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.Events
{
    [System.Serializable]
    public class JumpSound : MovementSound
    {
        [Space]
        [SerializeField] private AudioClip[] clips;

        public JumpSound(string name, JumpState state) : base(name, state) {

        }

        protected sealed override void Disable(MovementState state) {
            state?.RemoveOnEnteredListener(OnEntered);
        }

        protected sealed override void Enable(MovementState state) {
            state?.AddOnEnteredListener(OnEntered);
        }

        private void OnEntered()
        {
            AudioSource source = Source;

            if(source != null && clips != null)
            {
                AudioClip clip = clips.Length <= 0 ? null : clips[Random.Range(0, clips.Length)];
                if(clip != null) { source?.PlayOneShot(clip);}
            }
        }
    }
}
