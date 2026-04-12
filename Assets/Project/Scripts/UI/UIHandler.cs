using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;

public static class UIHandler 
{
    public static void InitializeButton(Button button, UnityAction onClick, string name)
    {
        if (button == null || onClick == null || string.IsNullOrEmpty(name)) return;
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(onClick);

        TextMeshProUGUI displayer = button.GetComponentInChildren<TextMeshProUGUI>();   
        if(displayer != null) displayer.text = name.ToUpper();
    }
}
