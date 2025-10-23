using TMPro;
using UnityEngine;

namespace RedSilver2.Framework.Player.Inventories.UI
{
    public abstract class HDItemInformationDisplayer : ItemInformationDisplayer {

       [SerializeField] private TextMeshProUGUI displayer;
        
        protected override void Awake() 
        {
            displayer = GetComponent<TextMeshProUGUI>();
            base.Awake();
        }

        protected sealed override void DisplayItemInformation(string message)
        {
            if (displayer != null) {
                displayer.text = message;
            }
        }
    }
}
