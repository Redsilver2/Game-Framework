using RedSilver2.Framework.Dialogs.Datas;
using System.Collections.Generic;
using UnityEngine;

namespace RedSilver2.Framework.Dialogs {
    [System.Serializable]
    public class CharacterSubtitle : Subtitle {

        [Space]
        [SerializeField] private string characterName;
        public string CharacterName => characterName;

        public CharacterSubtitle(string characterName) : base() { this.characterName = characterName; }
        public CharacterSubtitle(List<SubtitleData> datas, string characterName) : base(datas) { this.characterName = characterName; }
        public CharacterSubtitle(SubtitleData[] datas, string characterName) : base(datas) { this.characterName = characterName; }

        public override bool IsSimilar(Subtitle subtitle)
        {
            if (subtitle is not CharacterSubtitle) return base.IsSimilar(subtitle);
            return (subtitle as CharacterSubtitle).CharacterName
                   .Contains(characterName, System.StringComparison.OrdinalIgnoreCase);
        }
    }
}
