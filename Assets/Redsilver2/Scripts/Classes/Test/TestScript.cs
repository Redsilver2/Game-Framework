using RedSilver2.Framework.Inputs;
using RedSilver2.Framework.Dialogs;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    [SerializeField] private DialogInfo info;
    [SerializeField] private Transform parent;

    private bool flip = false;
    private int count = 0;

    private readonly string[] tests = new string[] {
        "This is a suprise for you :)",
        "If you are reading this, I hope you know that I am here for YOU",
        "I've been coding this for awhile now, but most of the system finally work :D",
        "I love you and thanks for supporting my projects <3",
        "This is a lot of dedication and work, you know"
    };

    private void Update()
    {
        if (InputManager.GetKeyDown(KeyboardKey.A)) {
            if (count >= tests.Length + 1) return;
            DialogManager.GetInstance()?.Play(parent, tests[count], 0.5f, 3f);
            count++;
        }
        else if (InputManager.GetKeyDown(KeyboardKey.Q)) {
            DialogManager.GetInstance()?.Play(info);
        }
    }
}
