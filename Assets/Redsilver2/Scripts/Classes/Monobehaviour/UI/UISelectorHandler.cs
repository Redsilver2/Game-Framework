using RedSilver2.Framework.UI;
using UnityEngine;

public class UISelectorHandler : MonoBehaviour
{
    [SerializeField] private UISelector defaultSelector;

    private void Start()
    {
        GameUIController.UpdateSelector(defaultSelector);
    }
}
