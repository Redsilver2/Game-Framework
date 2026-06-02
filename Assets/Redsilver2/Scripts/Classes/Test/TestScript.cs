using RedSilver2.Framework;
using RedSilver2.Framework.Inputs;
using RedSilver2.Framework.Subtitles;
using RedSilver2.Framework.Subtitles.Datas;
using Unity.VisualScripting;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    [SerializeField] private AudioClip clip;
    private DialogData data;
    private bool flip = false;
   
    void Start() {
        data = new DialogData("PHONE_CALL_NIGHT_1");

        data.AddSubtitle(new AudibleSubtitle(new SubtitleData[] {
            new SubtitleData("Hello? Hello hello?", 18.5f, 2f, 1f),
            new SubtitleData("Uh, I wanted to record a message for you, to help you get settled in on your first night.", 21f, 4.5f, 1f),
            new SubtitleData("Um, I actually worked in that office before you, I’m finishing up my last week now as a matter of fact.", 27f, 6f, 1f),
            new SubtitleData("So, I know it can be a bit overwhelming, but I’m here to tell you there’s nothing to worry about, Uh, you’ll do fine.", 33.5f, 7f, 1f),
            new SubtitleData("So, let’s just focus on getting you through your first week, okay?", 41f, 5f, 1f),
            new SubtitleData("Uh, let’s see, first there’s an introductory greeting from the company, that I’m supposed to read.", 47f, 4.5f, 1f),
            new SubtitleData("Uh, it’s kind of a legal thing, you know.", 52f, 2.5f, 1f)
        }, gameObject.GetOrAddComponent<AudioSource>(), clip));

        data.AddSubtitle(new CharacterSubtitle(new SubtitleData[] {
            new SubtitleData("Logan is pretty cool like guy, you know?", 10f, 5f, 1f),
            new SubtitleData("Wished I was awesome sauceand epic just like him!", 20f, 2.5f, 1f),
            new SubtitleData("I wasn't really sure about going outside untile now, you know?!", 25f, 4f, 1f)
        }, "John"));


    }

    private void Update()
    {
        if (InputManager.GetKeyDown(KeyboardKey.Space)) {
            flip = !flip;

            if (flip) {
                GameManager.SubtitleManager?.Play(data);
            }
            else {
                GameManager.SubtitleManager?.Stop(data);
            }
        }
    }
}
