using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.Extensions {
    [CreateAssetMenu(menuName = "Movement/Sounds/Land", fileName = "New Land Sound")]
    public sealed class LandSoundData : ScriptableObject {
        [SerializeField] private SoundData[] datas;

#if UNITY_EDITOR
        private void OnValidate() {
            List<SoundData> results = new List<SoundData>();

            foreach(GroundType type in Enum.GetValues(typeof(GroundType))) {
                if(datas == null) { results?.Add(new SoundData(type));  }
                else {
                    var similarDatas = datas.Where(x => x.groundType == type);
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
                    if (data.groundType.ToString().ToLower() == groundTag.ToLower())
                        return data.clips;
                }
            }

            return new AudioClip[0];
        }

        [System.Serializable]
        public struct SoundData {
            [HideInInspector] public GroundType groundType;
            public AudioClip[] clips;

            public SoundData(GroundType type)
            {
                groundType = type;
                clips = new AudioClip[0];
            }
        }
    }
}
