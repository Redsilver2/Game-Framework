using System;
using UnityEngine;
using static RedSilver2.Framework.Subtitles.Subtitle;

namespace RedSilver2.Framework.Subtitles
{
    public sealed class SubtitleManager : MonoBehaviour
    {
        public Subtitle test;

        [Space]
        [SerializeField] private SubtitleDisplayer template;
        [SerializeField] private DialogChoiceManager choiceManager;

        [Space]
        [SerializeField] private float subtitleWorldSpaceDistance = 10f;
        [SerializeField] private float minSubtitleFadeDistance = 20f;
        [SerializeField] private float maxSubtitleFadeDistance = 30f;

        [Space]
        [SerializeField] private float subtitleCatchupSpeed;

        [Space]
        [SerializeField] private bool canSubtitleUseWorldSpace;
        [SerializeField] private bool canShowSubtitleByTime;
        [SerializeField] private bool canDisplayCharacterName;

        public bool CanDisplayCharacterName => canDisplayCharacterName;
        public float SubtitleWorldSpaceDistance => subtitleWorldSpaceDistance;

        public float MinSubtitleFadeDistance => minSubtitleFadeDistance;
        public float MaxSubtitleFadeDistance => maxSubtitleFadeDistance;

        public float SubtitleCatchupSpeed => subtitleCatchupSpeed;
        public bool CanShowSubtitleContextByTime => canShowSubtitleByTime;

        public SubtitleDisplayer Template => template;

        public const string SCREEN_SPACE_ANCHOR = "Screen Space Main";

        private void Awake() {
        
        }

        private void Start()
        {
            SubtitleUpdater  updater = new StackableSubtitleUpdater(test != null ? test.GetSubtitleDatas() : null, SubtitleUpdater.SubtitleUpdaterType.ScreenSpace);
            updater?.SetDisplayerTemplate(template);
            updater?.Play();
        }
        public void Play(DialogInfo info) {
            if (info == null) return;
            Play(info.GetDialog());
        }

        public void Play(string name) {
           Play(Dialog.Get(name)); 
        }

        public void Play(Dialog dialog) 
        {

        }

        public void Stop(DialogInfo info)
        {
            if(info == null) return;
        }

        public static SubtitleManager GetInstance() {
            return GameManager.DialogManager;
        }

        public static DialogChoiceManager GetChoiceManager() {
            SubtitleManager manager = GetInstance();
            return manager != null ? manager.choiceManager : null;
        }
    }
}