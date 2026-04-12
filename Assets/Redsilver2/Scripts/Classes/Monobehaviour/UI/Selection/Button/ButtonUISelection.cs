using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace RedSilver2.Framework.UI
{
    public class ButtonUISelection : UISelection
    {
        private Button button;

        protected override void Awake()
        {
            base.Awake();
            button = GetComponentInChildren<Button>();
        }

        public void AddOnClickListener(UnityAction action) {
            if(action != null) button?.onClick.AddListener(action);
        }

        public void RemoveOnClickListener(UnityAction action)
        {
            if (action != null) button?.onClick.RemoveListener(action);
        }


        protected override IEnumerator UpdateCoroutine()
        {
            while(button != null)
            {
                if(GameUIController.GetConfirmState())
                    button?.onClick.Invoke();

                yield return null;
            }
        }
    }
}
