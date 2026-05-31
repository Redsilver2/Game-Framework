using RedSilver2.Framework;
using RedSilver2.Framework.Subtitles;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    [SerializeField] private Subtitle subtitle;
   
    void Start() {
        GameManager.SubtitleManager?.Play(subtitle);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
