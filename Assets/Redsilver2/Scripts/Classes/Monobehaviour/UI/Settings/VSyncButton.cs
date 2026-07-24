using RedSilver2.Framework.UI;
using TMPro;
using UnityEngine;


public sealed class VSyncButton : VSyncSettingUI
{
    [Space]
    [SerializeField] private UISelectionButton toggle;

    [Space]
    [SerializeField] private TextMeshProUGUI displayer;

    private void Start(){
        toggle?.AddOnClickListener(() => {
            SetIndex(Index == 0 ? (uint)1 : (uint)0);
        });
    }

    public sealed override void ApplySetting()
    {
        base.ApplySetting();
        if (displayer != null) displayer.text = GetDisplayedValue();
    }
}
