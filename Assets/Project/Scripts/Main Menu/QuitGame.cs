using RedSilver2.Framework.Inputs;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class QuitGame : MonoBehaviour
{
    [SerializeField] private GameObject mainPanel;

    [Space]
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;

    private bool isUIOpened;
    private IEnumerator openCoroutine;

  
    private void Awake() {
        InitializeYesButton();
        InitializeNoButton();
        isUIOpened = false;
    }

    void Update()
    {
        if (!isUIOpened) {
            UpdateInput();
        }
    }

    private void UpdateInput()
    {
        if (InputManager.GetKeyDown(KeyboardKey.Escape) || InputManager.GetKeyDown(GamepadButton.ButtonWest))
        {
            if (openCoroutine != null) StopCoroutine(openCoroutine);
            openCoroutine = OpenCoroutine();
            StartCoroutine(openCoroutine);

            SetMainPanelVisibility(true);
            isUIOpened = true;
        }
    }

    private void InitializeYesButton()
    {
        UIHandler.InitializeButton(yesButton, () =>  {
            Application.Quit();
        } ,"YES");
    }

    private void InitializeNoButton()
    {
        UIHandler.InitializeButton(noButton, () => 
        {
            if (openCoroutine != null) StopCoroutine(openCoroutine);
            openCoroutine = null;

           SetMainPanelVisibility(false);
           isUIOpened = false;
        }, "NO");
    }

    private void SetMainPanelVisibility(bool isVisible) {
        mainPanel?.SetActive(isVisible);    
    }

    private IEnumerator OpenCoroutine()
    {
        yield return null;
    }
}
