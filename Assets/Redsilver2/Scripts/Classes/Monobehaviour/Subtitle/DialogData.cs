using RedSilver2.Framework.Subtitles.Datas;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RedSilver2.Framework.Subtitles
{
    [System.Serializable]
    public struct DialogData {
        [SerializeField] private List<Subtitle> subtitles;
        public  readonly string name;

        public DialogData(string name) {
            this.name = name;
            subtitles = new List<Subtitle>();
        }

        public DialogData(List<Subtitle> subtitles, string name)
        {
            this.name = name;
            this.subtitles = subtitles != null ? subtitles : new List<Subtitle>();
        }

        public DialogData(Subtitle[] subtitles, string name) {
            this.name = name;
            this.subtitles = subtitles != null ? subtitles.ToList() : new List<Subtitle>();
        }

        public void AddSubtitle(Subtitle subtitle) {
            if (subtitle == null || subtitles == null || subtitles.Contains(subtitle)) return;
            subtitles?.Add(subtitle);
        }

        public void RemoveSubtitle(Subtitle subtitle) {
            if (subtitle == null || subtitles == null || !subtitles.Contains(subtitle)) return;
            subtitles?.Remove(subtitle);
        }

        public bool IsSimilar(DialogData data) {
           foreach(Subtitle subtitle in data.subtitles) {
               var results = data.subtitles.Where(x => x != null)
                                           .Where(X => X.IsSimilar(subtitle));

               if(results.Count() > 0) return true;
           }

           return data.name.Equals(name, System.StringComparison.OrdinalIgnoreCase);
        }

        public Subtitle GetSubtitle(int index) {
            if(subtitles == null || subtitles.Count <= 0 || index < 0 || index >= subtitles.Count) 
                return null;

            return subtitles[index];
        }

        public Subtitle[] GetSubtitles() {
            if (subtitles == null) return null;
            return subtitles.ToArray();
        }
    }
}
