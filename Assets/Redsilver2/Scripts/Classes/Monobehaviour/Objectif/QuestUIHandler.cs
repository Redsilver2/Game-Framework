using RedSilver2.Framework.Quests.Progressions;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Quests.UI {

    [RequireComponent(typeof(TextMeshProUGUI))]
    [RequireComponent(typeof(CanvasGroup))]
    public class QuestUIHandler : MonoBehaviour {
        [SerializeField] private QuestProgressionUIHandler template;
     
        private CanvasGroup group;
        private Quest owner;

        private TextMeshProUGUI mainDisplayer;
        private Dictionary<QuestProgression, QuestProgressionUIHandler> progressionUIHandlers;

        private UnityEvent<QuestProgressionUIHandler[]> onUpdateProgressionUIHandlers;

        private void Awake() {
            group = GetComponent<CanvasGroup>();       
            onUpdateProgressionUIHandlers = new UnityEvent<QuestProgressionUIHandler[]>();

            mainDisplayer = GetComponent<TextMeshProUGUI>();
            if (template != null) template.gameObject.SetActive(false);

            Debug.Log("what");
            AddOnUpdateProgressionUIHandlers(OnUpdateProgressionUIHandlers);
        }
        
        public void SetOwner(Quest quest)
        {

            Debug.Log(quest.Name);
            this.owner = quest;
            SetOwnerProgressionUI();
        }

        private void OnUpdateProgressionUIHandlers(QuestProgressionUIHandler[] handlers)
        {
            if (handlers == null) return;


            foreach (QuestProgressionUIHandler handler in handlers)
                handler?.UpdateHandler();
        }

        public void AddOnUpdateProgressionUIHandlers(UnityAction<QuestProgressionUIHandler[]> action)  {


            if (action != null) onUpdateProgressionUIHandlers?.AddListener(action);
        }
        public void RemoveOnUpdateProgressionUIHandlers(UnityAction<QuestProgressionUIHandler[]> action)
        {
            if (action != null) onUpdateProgressionUIHandlers?.RemoveListener(action);
        }

        public void UpdateHandler()
        {
            if (mainDisplayer != null && owner != null) 
                mainDisplayer.text = $"<size=40>{owner.Name}</size>\n{owner.Description}\n\n";

            if (progressionUIHandlers != null)
                onUpdateProgressionUIHandlers?.Invoke(progressionUIHandlers.Values.ToArray());
        }

        private void SetOwnerProgressionUI() {
            if(owner == null) return;

            if (progressionUIHandlers == null) {
                progressionUIHandlers = new Dictionary<QuestProgression, QuestProgressionUIHandler>();
            }
        
            if (template != null && owner.State == QuestState.Progressing)
            {
                foreach (QuestProgression progression in owner.GetProgressions())
                {
                    if (progression == null) continue;
                    progressionUIHandlers?.Add(progression, Instantiate(template));

                    progressionUIHandlers[progression].SetOwner(progression);
                    progressionUIHandlers[progression].transform.SetParent(transform);

                    progressionUIHandlers[progression].gameObject.SetActive(true);
                }
            }
        }  
    }
}
