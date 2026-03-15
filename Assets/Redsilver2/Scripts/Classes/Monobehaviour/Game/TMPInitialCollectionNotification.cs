using TMPro;
using UnityEngine;

namespace RedSilver2.Framework.Interactions.Collectibles
{
    public class TMPInitialCollectionNotification : InitialCollectibleNotification
    {
        private TextMeshProUGUI displayer;

        protected override void Awake()
        {
            base.Awake();
            displayer = GetComponentInChildren<TextMeshProUGUI>(true);  
        }

        protected sealed override void SetInformationsText(IPickableInteractable pickable)
        {
            if(pickable != null && displayer != null)
                displayer.text = $"{pickable.GetName()}\n\n{pickable.GetName()}";
        }
    }
}
