using RedSilver2.Framework.Quests.Datas;
using RedSilver2.Framework.Quests.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RedSilver2.Framework.Quests
{
    public class QuestManager : MonoBehaviour {
        [SerializeField] private QuestUIHandler template;
        [SerializeField] private Transform templateParent;

        [Space]
        [SerializeField] private Vector3 templatePosition;

        private Dictionary<Quest, QuestUIHandler> questUIHandlers;


        private void Awake() {
            questUIHandlers = new Dictionary<Quest, QuestUIHandler>();
            StartCoroutine(UpdateQuests());

            if (template != null) template.gameObject.SetActive(false);

            foreach (QuestData data in Resources.LoadAll<QuestData>("Quests"))
                data?.Instantiate();
        }

        private void OnEnable()
        {
            Quest.AddOnQuestStateChangedListener(OnQuestStateChanged);
        }

        private void OnDisable()
        {
            Quest.RemoveOnQuestStateChangedListener(OnQuestStateChanged);
        }

        private void OnQuestStateChanged(Quest quest)
        {
            if(quest == null || quest.State != QuestState.Progressing || questUIHandlers == null || questUIHandlers.ContainsKey(quest)) 
                return;

            if (template != null)
            {
                questUIHandlers?.Add(quest, Instantiate(template));
                questUIHandlers[quest]?.SetOwner(quest);
              
                questUIHandlers[quest].transform.SetParent(templateParent);
                questUIHandlers[quest].gameObject.SetActive(true);
            }
        }



        private IEnumerator UpdateQuests() {
            while (true) {
                Quest[] quests = Quest.GetActifQuests();

                if (quests == null) {
                    yield return null;
                    continue;
                }

               
                foreach (Quest quest in quests) {
                    quest?.Update();
                    if (questUIHandlers == null || !questUIHandlers.ContainsKey(quest) || questUIHandlers[quest] == null) continue;

                    questUIHandlers[quest].transform.localPosition = templatePosition;
                    questUIHandlers[quest]?.UpdateHandler();
                }

                yield return null;
            }
        }
    }
}
