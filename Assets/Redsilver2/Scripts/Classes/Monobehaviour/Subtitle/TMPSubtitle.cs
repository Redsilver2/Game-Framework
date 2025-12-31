using TMPro;
using UnityEngine;

namespace RedSilver2.Framework.Subtitles
{
    public class TMPSubtitle : Subtitle
    {
        [SerializeField] private TextMeshProUGUI contextDisplayer;
        [SerializeField] private TextMeshProUGUI characterNameDisplayer;

        protected override void UpdateDisplayers(string characterName, string context)
        {
            if(characterNameDisplayer != null) {
                characterNameDisplayer.text = characterName;
                if (contextDisplayer != null) contextDisplayer.text = context;
            }
            else {
                if (contextDisplayer != null) contextDisplayer.text = $"{characterName} {context}";
            }
        }
    }
}
