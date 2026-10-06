using RedSilver2.Framework;
using System.Threading;
using TMPro;
using UnityEngine;

public class TestTextTagScript : MonoBehaviour
{
    public TMP_Text displayer;

    private void Start()
    {
        CancellationTokenSource source = new CancellationTokenSource();
        StartCoroutine(TextTagManager.Instance.UpdateDisplayer(displayer, "<randomunitcircle()>Wow I didnt know that this was possible", 5f, source.Token));
    }
}
