using RedSilver2.Framework;
using RedSilver2.Framework.Inputs;
using RedSilver2.Framework.Dialogs;
using RedSilver2.Framework.Dialogs.Datas;
using UnityEngine;
using Unity.VisualScripting;

public class TestScript : MonoBehaviour
{
    [SerializeField] private DialogInfo info;
    [SerializeField] private AudibleSubtitleInfo subtitleInfo;
    private bool flip = false;
   
    void Start() {
        if (subtitleInfo != null) subtitleInfo.Subtitle?.SetAudioSource(GetComponent<AudioSource>());
    }

    private void Update()
    {
        if (InputManager.GetKeyDown(KeyboardKey.A)) {
            flip = !flip;

            if (flip) { GameManager.DialogManager?.Play(info); }
            else {
                GameManager.DialogManager?.Stop();
            }
        }
    }
}
