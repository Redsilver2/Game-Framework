using RedSilver2.Framework.Inputs;
using TMPro;
using UnityEngine;

public class TestScript : MonoBehaviour
{


    private string currentText;
    private float currentUpdateDelay;
    [SerializeField] private TextMeshProUGUI displayer;
    [SerializeField] private float UPDATE_DELAY = 0.05f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentText = string.Empty; 
        currentUpdateDelay = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        currentUpdateDelay = Mathf.Clamp(currentUpdateDelay - Time.deltaTime, 0f, UPDATE_DELAY);

        if (currentUpdateDelay <= 0f) {
            if(InputManager.Write(ref currentText)) {
                currentUpdateDelay = UPDATE_DELAY;
            }
        }

        if(displayer != null) displayer.text = currentText;
    }
}
