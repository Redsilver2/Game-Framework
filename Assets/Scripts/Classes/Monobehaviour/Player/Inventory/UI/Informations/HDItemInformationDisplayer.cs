using TMPro;
using UnityEngine;

namespace RedSilver2.Framework.Player.Inventories.UI
{
    public abstract class HDItemInformationDisplayer : ItemInformationDisplayer {

       [SerializeField] private TextMeshProUGUI displayer;
        
        protected sealed override void Awake() 
        {
            if (displayer != null) displayer.text = string.Empty;
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
