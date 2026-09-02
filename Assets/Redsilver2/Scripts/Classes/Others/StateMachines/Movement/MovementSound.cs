using RedSilver2.Framework.StateMachines.Extensions;
using RedSilver2.Framework.StateMachines.States;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.Events
{
    [System.Serializable]
    public abstract class MovementSound : MovementStateEvent
    {
        [Space]
        [SerializeField] private AudioSource source;

        [Space]
        [SerializeField] private MovementSoundData data;

        protected MovementSoundData Data => data;
        protected AudioSource Source => source;

        protected MovementSound(string name, MovementState state) : base(name, state) {
        
        }

        public void SetSource(AudioSource source)
        {
            this.source = source;
        }

        public void SetData(MovementSoundData data)
        {
            this.data = data;
        }
    }
}