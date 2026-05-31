using RedSilver2.Framework.Subtitles.Datas;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;


namespace RedSilver2.Framework.Subtitles
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class Subtitle : ScriptableObject {
        [SerializeField] private string subtitleName;
        public string SubtitleName => subtitleName;

        public Subtitle() {  
        
        }

        public SubtitleData GetSubtitleData(int index)
        {
            SubtitleData[] results = GetSubtitleDatas();

            if(results == null || results.Length == 0 || index < 0 || index >= results.Length)
                return null;

            return results[index];  
        }

        public SubtitleData[] GetSubtitleDatas(int[] indexes)
        {
            List<SubtitleData> results = new List<SubtitleData>();
            if (indexes == null) return null;

            for (int i = 0; i < indexes.Length; i++) 
                results.Add(GetSubtitleData(indexes[i]));   

            return results.Where(x => x != null).ToArray();
        }


        public abstract SubtitleData[] GetSubtitleDatas();
    }
}
