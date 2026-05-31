using UnityEngine;

namespace RedSilver2.Framework.Subtitles.Datas
{
    public class CharacterSubtitleData : SubtitleData {
        [Space]
        [SerializeField] private string characterName;
        public string CharacterName => characterName;
    }
}
