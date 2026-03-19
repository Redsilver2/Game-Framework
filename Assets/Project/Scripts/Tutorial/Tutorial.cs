using System.Collections;
using TMPro;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI displayer;
    [SerializeField] private string[] messages;

    private IEnumerator showMessages;
    private const float MESSAGE_SHOW_TIME = 1f;

    private void Start()
    {
        showMessages = ShowMessages();
        StartCoroutine(showMessages);
    }

    private void OnDisable()
    {
        StopCoroutine(showMessages);
    }

    private void SetDisplayerVisibility(bool isVisible)
    {
        if (displayer != null) 
            displayer.gameObject.SetActive(isVisible);
    }

    private void SetDisplayerText(string text) {
        if(displayer != null)  displayer.text = text;
    }

    private IEnumerator ShowMessages()
    {
        WaitForSeconds wait = new WaitForSeconds(MESSAGE_SHOW_TIME);
        SetDisplayerVisibility(true);

        for (int i = 0; i < messages.Length; i++)
        {
            SetDisplayerText(messages[i]);
            yield return wait;
        }

        SetDisplayerVisibility(false);
    }
}
