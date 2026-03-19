using System.Collections;
using TMPro;
using UnityEngine;

public abstract class OrbCollectionUIHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI orbCountDisplayer;
    private CustomGameManager gameManager;

    private IEnumerator updateOrbCountDisplayer;

    private void Awake() {
        gameManager = CustomGameManager.GetInstance();
    }

    private void OnEnable()
    {
        gameManager?.AddOnOrbCollectedListener(OnOrbCollected);
    }

    private void OnDisable()
    {
        gameManager?.RemoveOnOrbCollectedListener(OnOrbCollected);
        StopCoroutine(updateOrbCountDisplayer);
        SetOrbCountDisplayerVisibility(false);
    }



    private void OnOrbCollected(int orbsLeft)
    {
        if (orbsLeft <= 0) return;

        if(updateOrbCountDisplayer != null) 
            StopCoroutine(updateOrbCountDisplayer);

        updateOrbCountDisplayer = UpdateOrbCountDisplayerCoroutine(orbsLeft);
        StartCoroutine(updateOrbCountDisplayer);
    }

    private void SetOrbCountDisplayerVisibility(bool isVisible)
    {
        if (orbCountDisplayer != null)
            orbCountDisplayer.enabled = isVisible;
    }

    private IEnumerator UpdateOrbCountDisplayerCoroutine(int count)
    {
        string message = GetDisplayMessage(count);
        SetOrbCountDisplayerVisibility(true);

        while(orbCountDisplayer != null) {
            orbCountDisplayer.text = message;
            yield return null;
        }

        SetOrbCountDisplayerVisibility(false);
    }

    protected abstract string GetDisplayMessage(int count);
}
