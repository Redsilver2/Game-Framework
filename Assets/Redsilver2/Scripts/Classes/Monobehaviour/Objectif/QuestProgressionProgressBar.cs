using RedSilver2.Framework.Quests.Progressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RedSilver2.Framework.Quests.UI
{
    [RequireComponent(typeof(Image))]
    public class QuestProgressionProgressBar : MonoBehaviour {
        [SerializeField] private float updateSpeed;
        [SerializeField] private float spacing;

        private Image progressBar;
        private QuestProgressionUIHandler handler;

        private void Awake() {
            progressBar = GetComponent<Image>();
            handler = transform.parent.GetComponent<QuestProgressionUIHandler>();
        }
        private void Start() { handler?.AddOnUpdateListener(OnUpdate); }

        private void OnEnable() { handler?.AddOnUpdateListener(OnUpdate); }
        private void OnDisable() { handler?.RemoveOnUpdateListener(OnUpdate); }

        private void OnUpdate()
        {
            if(handler != null) {
                QuestProgression progression = handler.Owner;
                bool             wasUpdated  = false;

                if(progression is UIntQuestProgression) OnUpdate(progression as UIntQuestProgression, out wasUpdated);

                if(progressBar != null) progressBar.gameObject?.SetActive(wasUpdated);
                OnUpdate(handler.DescriptionDisplayer);
            }
        }
        private void OnUpdate(UIntQuestProgression progression, out bool wasUpdated)
        {
            wasUpdated = false;

            if (progression == null || handler == null || progressBar == null) return;
            progressBar.color      = Color.Lerp(progressBar.color, handler.GetColorFromState(), Time.deltaTime * updateSpeed);
          
            progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, progression.GetProgress(), Time.deltaTime * updateSpeed);
            wasUpdated = true;
        }
        private void OnUpdate(TextMeshProUGUI displayer) {
            if(displayer == null) return;
            transform.position = Vector3.Lerp(transform.position, 
                                              displayer.transform.position + Vector3.down * (spacing * displayer.textInfo.lineCount), 
                                              Time.deltaTime * updateSpeed);        
        }
    }
}
