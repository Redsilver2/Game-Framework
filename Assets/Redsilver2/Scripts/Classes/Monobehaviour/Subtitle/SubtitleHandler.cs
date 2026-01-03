using RedSilver2.Framework.Player;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;


namespace RedSilver2.Framework.Subtitles
{
    [RequireComponent(typeof(Canvas))]
    public class SubtitleHandler : MonoBehaviour
    {
        [SerializeField] private Vector3 defaultPosition;

        [Space]
        [SerializeField] private float maxDistanceCheck;
        [SerializeField] private float distanceBetweenSubtitles;

        [Space]
        [SerializeField] private bool isExcludedFromWorldSpaceCheck;

        private bool isUpdatingSubtitles;

        private Queue<Subtitle> subtitles;
        private List <Subtitle> actifSubtitles; 

        private async void Awake() {
            subtitles       = new Queue<Subtitle>();
            actifSubtitles  = new List<Subtitle>();

            Canvas canvas = GetComponent<Canvas>();
            canvas.renderMode  = RenderMode.WorldSpace;
            canvas.worldCamera = Camera.main;

            Play("John", "Let's play a game called hide and seek!", 1.5f, 1f, false);
            await Awaitable.WaitForSecondsAsync(1.5f);
            Play("John", "You will have 10 seconds to hide :) and you will have to hide.", 0.5f, 1f, false);
            await Awaitable.WaitForSecondsAsync(1.5f);
            Play("John", "I'll start count now!", 1.5f, 1f, false);

            for (int i = 1; i <= 10; i++) {
                await Awaitable.WaitForSecondsAsync(1f);
                Play("John", $"{i}!", 0.25f, 1.5f, false);
            }

            Play("John", $"Ready or not here I come :D", 1.5f, 1.5f, false);
        }

        public void Play(string characterName, string contextText, float duration, float fadeDelay, bool isDescription) {
            Subtitle subtitle = GetActifSubtitle();
            if (subtitle == null) return;

            subtitle.SetSubtitleHandler(this);

            if (!isDescription) {
                subtitle.Play(characterName, contextText, duration, fadeDelay);
            }
            else {
                subtitle.PlayDescription(characterName, contextText, duration, fadeDelay);
            }

            if (!isUpdatingSubtitles)
            {
                StartCoroutine(UpdateSubtitle());
            }
        }


        public void PlayCaption(string characterName, string contextText, float duration, float fadeDelay)
        {
            Subtitle subtitle = GetActifSubtitle();
            if (subtitle == null) return;

            subtitle.SetSubtitleHandler(this);
            subtitle.PlayDescription(characterName, contextText, duration, fadeDelay);

            if (!isUpdatingSubtitles){
                StartCoroutine(UpdateSubtitle());
            }
        }

        public void RemoveActifSubtitle(Subtitle subtitle)
        {
            if (subtitle == null || actifSubtitles == null || !actifSubtitles.Contains(subtitle)) return;
            actifSubtitles.Remove(subtitle);    
        }

        private  IEnumerator UpdateSubtitle() {

            isUpdatingSubtitles = true;

            while (actifSubtitles.Count > 0) {
                SubtitleManager  subtitleManager = SubtitleManager.Instance;
                PlayerController player          = PlayerController.Current;
                Subtitle[]       subtitles       = actifSubtitles.Where(x => x != null).ToArray();

                if (player == null || subtitleManager == null || subtitles == null) {
                    yield return null;
                    continue;
                }

                if (!subtitleManager.CanSubtitleUseWorldSpace || isExcludedFromWorldSpaceCheck)
                    yield return StartCoroutine(ScreenSpaceSubtitleUpdate(subtitleManager, subtitles));
                else {
                    Debug.Log("Distance: " + Vector3.Distance(transform.position, player.transform.position));

                    if (Vector3.Distance(transform.position, player.transform.position) >= maxDistanceCheck)
                        yield return StartCoroutine(ScreenSpaceSubtitleUpdate(subtitleManager, subtitles));
                    else
                        yield return StartCoroutine(WorldSpaceSubtitleUpdate(player, subtitleManager, subtitles));
                }

                yield return null;
            }

            isUpdatingSubtitles = false;
        }

        private IEnumerator ScreenSpaceSubtitleUpdate(SubtitleManager subtitleManager, Subtitle[] subtitles) {
            if (subtitles != null && subtitleManager != null){
                foreach (Subtitle subtitle in subtitles.Where(x => x != null)) subtitleManager.AddScreenSubtitle(subtitle);
            }

            yield return null;
        }

        private IEnumerator WorldSpaceSubtitleUpdate(PlayerController player, SubtitleManager subtitleManager, Subtitle[] subtitles)
        {
            if (subtitles == null || player == null || subtitleManager == null) yield return null;

            subtitles = subtitles.Where(x => x != null).ToArray();
            yield return StartCoroutine(subtitleManager.UpdateScreenSubtitles(distanceBetweenSubtitles, defaultPosition, transform, subtitles)); 
        }

        private Subtitle GetActifSubtitle()
        {
            bool foundSubtitle = false;

            if(subtitles != null) {
                if (subtitles.Count > 0) {
                    actifSubtitles.Add(subtitles.Dequeue());
                    foundSubtitle = true;
                }
            }

            SubtitleManager subtitleManager = SubtitleManager.Instance;

            if (subtitleManager != null && !foundSubtitle) {
                actifSubtitles.Add(subtitleManager.GetSubtitle());
                foundSubtitle = true;
            }

            if(foundSubtitle) return actifSubtitles[actifSubtitles.Count - 1];
            return null;
        }

    }
}
