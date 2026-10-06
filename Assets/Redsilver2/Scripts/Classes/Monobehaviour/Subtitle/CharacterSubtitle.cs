using UnityEngine;

namespace RedSilver2.Framework.Subtitles {
    [System.Serializable]
    public class CharacterSubtitle : Subtitle {

        [Space]
        [SerializeField] private string characterName;
        public string CharacterName => characterName;

        public CharacterSubtitle(string characterName) : base() { this.characterName = characterName; }
    }
}
