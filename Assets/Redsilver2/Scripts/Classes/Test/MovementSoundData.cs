using UnityEngine;

namespace RedSilver2.Framework.StateMachines.Extensions
{
    [CreateAssetMenu(fileName = "New Movement Sound Datas", menuName = "Movement Sound Datas")]
    public class MovementSoundData : ScriptableObject
    {
        [SerializeField] private GroundSoundData[] datas;



        public void PlaySound(AudioClip clip) {

        }

        [System.Serializable]
        private class GroundSoundData {

        }
    }
}
