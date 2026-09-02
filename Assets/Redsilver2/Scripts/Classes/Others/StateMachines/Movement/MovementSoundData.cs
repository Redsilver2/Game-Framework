using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.Extensions {
    [CreateAssetMenu(menuName = "Movement/Sounds/Land", fileName = "New Land Sound")]
    public sealed class MovementSoundData : ScriptableObject {
        [SerializeField] private SoundData[] datas;

#if UNITY_EDITOR
        private void OnValidate() {
            List<SoundData> results = new List<SoundData>();

            foreach(GroundType type in Enum.GetValues(typeof(GroundType))) {
                if(datas == null) { results?.Add(new SoundData(type));  }
                else {
                    var similarDatas = datas.Where(x => x != null).Where(x => x.name.ToLower() == type.ToString().ToLower());
                    results.Add(similarDatas.Count() > 0 ? similarDatas.First() : new SoundData(type));
                }
            }

            datas = results.ToArray();
        }
#endif

        public AudioClip[] GetClips(string groundTag)
        {
            if(datas != null && !string.IsNullOrEmpty(groundTag)) {
                groundTag = groundTag.ToLower();
                
                foreach (SoundData data in datas) {
                    if (data.name.ToLower() == groundTag.ToLower())
                        return data.Clips;
                }
            }

            return new AudioClip[0];
        }

        [System.Serializable]
        public class SoundData {
            [HideInInspector] public string name;
            [SerializeField] private AudioClip[] clips;
            public AudioClip[] Clips => clips;

            public SoundData(GroundType type)
            {
                name       = type.ToString();
                clips      = new AudioClip[0];
            }
        }
    }
}
