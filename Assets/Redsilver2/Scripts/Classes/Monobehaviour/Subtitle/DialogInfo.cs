using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RedSilver2.Framework.Dialogs
{
    [CreateAssetMenu(fileName = "New Dialog Info", menuName = "Dialog/Info/Default")]
    public sealed class DialogInfo : ScriptableObject {
        [SerializeField] private SubtitleInfo[] subtitleInfos;
        [SerializeField] private DialogChoiceInfo[] choiceInfos;

        public Dialog GetDialog() {
            Dialog result = Dialog.Get(name);

            if (result != null) return result;
            else if (string.IsNullOrEmpty(name)) return null;

            return new Dialog(GetSubtitles(), GetChoices(), name);
        }

        private Subtitle[] GetSubtitles() {
            List<Subtitle> results = new List<Subtitle>();
            if (subtitleInfos == null) return results.ToArray();

            foreach (SubtitleInfo info in subtitleInfos.Distinct()) {
                if (info == null) continue;
                Subtitle subtitle = info.GetSubtitle();

                if (subtitle == null) continue;
                results.Add(subtitle);
            }

            return results.ToArray();
        }

        private DialogChoice[] GetChoices() {
            List<DialogChoice> results = new List<DialogChoice>();
            if(choiceInfos == null) return results.ToArray();

            foreach (DialogChoiceInfo info in choiceInfos.Distinct()) {
                if (info == null) continue;
                DialogChoice choice = info.GetChoice();

                if (choice == null) continue;
                results?.Add(choice);
            }

            return results.ToArray();
        }
    }
}
