using TMPro;
using UnityEngine;

namespace RedSilver2.Framework.Subtitles
{
    public sealed class TMPSubtitle : Subtitle
    {
        [SerializeField] private TextMeshProUGUI contextDisplayer;
        [SerializeField] private TextMeshProUGUI characterNameDisplayer;

        public sealed override int GetLineCount()
        {
            if(contextDisplayer == null) {
                return 0;
            }

            return contextDisplayer.textInfo.lineCount;
        }

        protected sealed override void UpdateDisplayers(string characterName, string context)
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
