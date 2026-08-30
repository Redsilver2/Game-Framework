using RedSilver2.Framework.StateMachines.Extensions;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines
{
    public abstract class GroundSoundData
    {
        [SerializeField] private GroundType groundType;
        [SerializeField] private AudioClip[] clips;

        public void PlaySound() {

        }
    }
}
