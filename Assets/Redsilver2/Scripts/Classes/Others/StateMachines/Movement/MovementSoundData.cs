using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.Extensions {
    [CreateAssetMenu(menuName = "Movement/Sounds/Land", fileName = "New Land Sound")]
    public sealed class MovementSoundData : ScriptableObject {
        [SerializeField] private Dictionary<string, AudioClip[]> datas;

#if UNITY_EDITOR
        private void OnValidate() {

            GroundType[] types = Enum.GetValues(typeof(GroundType)) as GroundType[];
            if (types == null) return;
            else if (datas == null || datas.Count != types.Length) {
                Dictionary<string, AudioClip[]> results = new Dictionary<string, AudioClip[]>();

                foreach (GroundType type in types) {
                    string _type = type.ToString();

                    if (datas == null || !datas.ContainsKey(_type)) { results?.Add(_type, new AudioClip[0]); }
                    else { results?.Add(_type, datas[_type]); }
                }

                datas = results;
            }
        }
#endif

        public AudioClip[] GetClips(string groundTag)
        {
            if(datas != null && !string.IsNullOrEmpty(groundTag)) {
                if(datas.ContainsKey(groundTag)) return datas[groundTag];
            }

            return new AudioClip[0];
        }
    }
}
