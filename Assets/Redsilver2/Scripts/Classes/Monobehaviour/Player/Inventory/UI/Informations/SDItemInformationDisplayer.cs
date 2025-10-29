using RedSilver2.Framework.Interactions.Items;
using UnityEngine;
using UnityEngine.UI;

namespace RedSilver2.Framework.Player.Inventories.UI
{
    [RequireComponent(typeof(Text))]
    public abstract class SDItemInformationDisplayer : ItemInformationDisplayer
    {
        [SerializeField] private Text displayer;

        protected sealed override void Awake() {
            displayer = GetComponent<Text>();   
            base.Awake();
        }

        protected sealed override void DisplayItemInformation(string message)
        {
            if (displayer != null){
                displayer.text = message;
            }
        }
    }
}
