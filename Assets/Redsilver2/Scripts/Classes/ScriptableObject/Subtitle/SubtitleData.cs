using RedSilver2.Framework.References;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace RedSilver2.Framework.Subtitles
{
    public partial class Subtitle : ScriptableObject
    {
        [System.Serializable]
        public class SubtitleData
        {
            [SerializeField, HideInInspector] private GameTime startTime;
            [SerializeField, HideInInspector] private GameTime endTime;

            [SerializeField, HideInInspector] private float fadeInDuration;

            [Space]
            [SerializeField, HideInInspector] private float fadeOutWaitTime;
            [SerializeField, HideInInspector] private float fadeOutDuration;

            [Space]
            [SerializeField, HideInInspector] private string anchorName;

            [Space]
            [SerializeField, HideInInspector] private string context;

            private bool isUpdateStarted;
            private bool isUpdateFinished;
            private SubtitleDisplayer displayer;
            private CancellationTokenSource fadeTokenSource;

            public bool IsUpdateStarted  => isUpdateStarted;
            public bool IsUpdateFinished => isUpdateFinished;

            public float FadeInDuration  => fadeInDuration;
            public float FadeOutDuration => fadeOutDuration;

            public string Context => context;
            public string AnchorName => anchorName;

            public float StartTime => startTime.GetFloatConversion();
            public float EndTime   => endTime.GetFloatConversion();


            public float Duration
            {
                get
                {
                    return Mathf.Clamp(endTime.GetFloatConversion() - startTime.GetFloatConversion(),
                                       0f,
                                       float.MaxValue);
                }
            }
            public SubtitleDisplayer Displayer => displayer;
            public SubtitleData()
            {
                this.context = string.Empty;
                this.anchorName = string.Empty;

                this.isUpdateStarted = false;
                this.isUpdateFinished = false;

                startTime = new GameTime();
                endTime = new GameTime();
            }


            public SubtitleData(string context)
            {
                this.context = context;
                this.anchorName = string.Empty;

                this.isUpdateStarted = false;
                this.isUpdateFinished = false;

                startTime = new GameTime();
                endTime = new GameTime();
            }

            public SubtitleData(string context, string anchorName)
            {
                this.context = context;
                this.anchorName = anchorName;

                this.isUpdateStarted = false;
                this.isUpdateFinished = false;

                startTime = new GameTime();
                endTime = new GameTime();
            }

            public SubtitleData(string context, GameTime startTime)
            {
                this.context = context;
                this.anchorName = string.Empty;

                this.isUpdateStarted = false;
                this.isUpdateFinished = false;

                this.startTime = startTime;
                endTime = new GameTime();
            }

            public SubtitleData(string context, string anchorName, GameTime startTime)
            {
                this.context = context;
                this.anchorName = anchorName;

                this.isUpdateStarted = false;
                this.isUpdateFinished = false;

                this.startTime = startTime;
                endTime = new GameTime();
            }

            public SubtitleData(string context, GameTime startTime, GameTime endTime)
            {
                this.context   = context;
                this.startTime = startTime;

                this.isUpdateStarted  = false;
                this.isUpdateFinished = false;

                this.anchorName = string.Empty;
                this.endTime    = endTime;
            }

            public SubtitleData(string context, string anchorName, GameTime startTime, GameTime endTime)
            {
                this.context   = context;
                this.startTime = startTime;

                this.isUpdateStarted = false;
                this.isUpdateFinished = false;

                this.anchorName = anchorName;
                this.endTime    = endTime;
            }

            public IEnumerator Update(SubtitleUpdater updater, SubtitleDisplayer displayer, CancellationToken token)
            {
                if (updater != null && !isUpdateStarted) {
                    if (updater.TimeElapsed >= StartTime) {

                        fadeTokenSource?.Cancel();
                        fadeTokenSource = new CancellationTokenSource();

                        CancellationToken fadeToken = fadeTokenSource.Token; 
                        CoroutineManager.Start(FadeDisplayer(displayer, updater, true, fadeToken));

                        float timeElapsed = Mathf.Clamp(StartTime <= 0f ? EndTime : updater.TimeElapsed - StartTime, 0f, float.MaxValue);
                        isUpdateStarted = true;

                        yield return CoroutineManager.Start(Update(timeElapsed, displayer, updater, token));
                        isUpdateFinished = true;

                        CoroutineManager.Start(FadeDisplayer(displayer, updater, false, fadeToken));
                    }
                }
            }

            private IEnumerator FadeDisplayer(SubtitleDisplayer displayer, SubtitleUpdater updater, bool isVisible, CancellationToken token) {
                if(displayer != null && updater != null) {
                    if (!isVisible)
                    {
                        float t = 0f;

                        while (!token.IsCancellationRequested)
                        {
                            if (t >= fadeOutWaitTime) break;
                            t += Time.deltaTime;
                            yield return null;
                        }
                    }

                    yield return CoroutineManager.Start(FadeDisplayer(displayer, isVisible, token));
                }
            }

            private IEnumerator FadeDisplayer(SubtitleDisplayer displayer, bool isVisible, CancellationToken token) {
                if(displayer != null) {
                    float currentAlpha = displayer.GetAlpha();
                    float desiredAlpha = isVisible ? 1f : 0f;
                    float t = 0f;

                    while (!token.IsCancellationRequested) {
                        float targetTime = (isVisible ? fadeInDuration : fadeOutDuration);
                        float progress   = Mathf.Clamp01(targetTime <= 0f ? 1f : t / targetTime);
                        
                        if(progress >= 1f) {
                            displayer?.SetAlpha(desiredAlpha);
                            break;
                        }

                        displayer?.SetAlpha(Mathf.Lerp(currentAlpha, desiredAlpha, progress));
                        t += Time.deltaTime;

                        yield return null;
                    }          
                }
            }

            public void Reset()
            {
                isUpdateStarted   = false;
                isUpdateFinished  = false;
            }

            protected virtual IEnumerator Update(float timeElapsed, SubtitleDisplayer displayer, SubtitleUpdater updater, CancellationToken token) {
                SubtitleManager manager = SubtitleManager.GetInstance();

                while (!token.IsCancellationRequested && displayer != null && updater != null) {
                    Update(displayer, manager, Mathf.Clamp01(timeElapsed / Duration));
                    if (timeElapsed >= Duration) break;

                    timeElapsed += Time.deltaTime;
                    yield return null;
                }
            }

            protected virtual void Update(SubtitleDisplayer displayer, SubtitleManager manager, float progress)
            {
                displayer?.DisplayContext(context, progress, manager);
            }

#if UNITY_EDITOR

            [SerializeField, HideInInspector] private bool showBaseFoldout;
            [SerializeField, HideInInspector] private bool useWorldSpaceAnchor;

            private void Validate()
            {
                startTime?.Validate();
                endTime?.Validate();

                if (endTime.GetFloatConversion() < startTime.GetFloatConversion()) {
                    endTime.SetHours(startTime.Hours);
                    endTime?.SetMinutes(startTime.Minutes);
                    endTime?.SetSeconds(startTime.Seconds);
                }
            }

            public void DrawInspector(string name, ref bool removeData)
            {
                Validate();

                name += $"   ({startTime.ToString()}) - " +
                        $"({endTime.ToString()})";

                if (EditorExtension.DisplayFoldout(name, ref showBaseFoldout, new Color32(0, 0, 0, 181)))
                {
                    bool result = false;

                    EditorExtension.DisplayButton("Remove", () => {  result = true; });
                    EditorExtension.Space(10f);

                    EditorExtension.DrawVerticalHelpBox(() => {
                        DrawBaseSettings();
                    }, true);

                    removeData = result;
                }


            }

            protected virtual void DrawBaseSettings()
            {
                startTime?.DrawInspector("Start Time");
                endTime?.DrawInspector("End Time");

                EditorExtension.Space(10f);
                fadeInDuration = EditorExtension.DisplayFloatSlider("Fade In Duration", fadeInDuration, 0f, 100f);

                EditorExtension.Space(10f);
                fadeOutWaitTime = EditorExtension.DisplayFloatSlider("Fade Out Wait Time", fadeOutWaitTime, 0f, 100f);
                fadeOutDuration = EditorExtension.DisplayFloatSlider("Fade Out Duration", fadeOutDuration, 0f, 100f);

                EditorExtension.Space(10f);
                useWorldSpaceAnchor = EditorExtension.DisplayToggle("Use World Space Anchor", useWorldSpaceAnchor);

                if (!useWorldSpaceAnchor) anchorName = SubtitleManager.SCREEN_SPACE_ANCHOR;
                else
                {
                    EditorExtension.DrawHorizontal(() => {
                        EditorExtension.DisplayBoldLabel("Anchor Name");
                        anchorName = EditorGUILayout.TextField(anchorName).ToLower();
                    });
                }


                EditorExtension.Space(10f);

                EditorExtension.DrawHorizontal(() => {
                    EditorExtension.DisplayBoldLabel("Context");
                    context = EditorGUILayout.TextArea(context);
                });
            }
#endif
        }

        [System.Serializable]
        public sealed class AudibleSubtitleData : SubtitleData
        {
            [Space]
            [SerializeField] private AudioClip clip;

            [Space]
            [SerializeField] private string audioSourceReferenceName;

            public AudioClip Clip => clip;

            public AudibleSubtitleData() : base() { 
                this.clip = null;
                audioSourceReferenceName = string.Empty;
            }
            public AudibleSubtitleData(string context) : base(context)
            {
                this.clip = null;
                audioSourceReferenceName = string.Empty;
            }

            public AudibleSubtitleData(string context, AudioClip clip) : base(context)
            {
                this.clip = clip;
                audioSourceReferenceName = string.Empty;
            }

            public AudibleSubtitleData(string context, GameTime startTime) : base(context, startTime)
            {
                this.clip = null;
                audioSourceReferenceName = string.Empty;
            }

            public AudibleSubtitleData(string context, AudioClip clip, GameTime startTime) : base(context, startTime)
            {
                this.clip = clip;
                audioSourceReferenceName = string.Empty;
            }

            public AudibleSubtitleData(string context, GameTime startTime, GameTime endTime) : base(context, startTime, endTime)
            {
                this.clip = null;
                audioSourceReferenceName = string.Empty;
            }

            public AudibleSubtitleData(string context, AudioClip clip, GameTime startTime, GameTime endTime) : base(context, startTime, endTime)
            {
                this.clip = clip;
                audioSourceReferenceName = string.Empty;
            }

            public AudibleSubtitleData(string context, string anchorName) : base(context, anchorName)
            {
                audioSourceReferenceName = string.Empty;
            }

            public AudibleSubtitleData(string context, string anchorName, AudioClip clip) : base(context, anchorName)
            {
                audioSourceReferenceName = string.Empty;
            }

            public AudibleSubtitleData(string context, string anchorName, GameTime startTime) : base(context, anchorName, startTime)
            {
                audioSourceReferenceName = string.Empty;
            }

            public AudibleSubtitleData(string context, string anchorName, AudioClip clip, GameTime startTime) : base(context, anchorName, startTime)
            {
                audioSourceReferenceName = string.Empty;
            }

            public AudibleSubtitleData(string context, string anchorName, GameTime startTime, GameTime endTime) : base(context, anchorName, startTime, endTime)
            {
                audioSourceReferenceName = string.Empty;
            }

            public AudibleSubtitleData(string context, string anchorName, AudioClip clip, GameTime startTime, GameTime endTime) : base(context, anchorName, startTime, endTime)
            {
                audioSourceReferenceName = string.Empty;
            }

            protected override IEnumerator Update(float timeElapsed, SubtitleDisplayer displayer, SubtitleUpdater updater, CancellationToken token)
            {
                AudioSource source = AudioSourceReference.GetSource(audioSourceReferenceName);

                if (source != null) {
                    source.clip = this.clip;
                    source?.Play();

                    if(this.clip != null) source.time = timeElapsed;
                }

                yield return CoroutineManager.Start(base.Update(timeElapsed, displayer, updater, token));
                source?.Stop();
            }

#if UNITY_EDITOR
            protected sealed override void DrawBaseSettings()
            {
                base.DrawBaseSettings();

                EditorExtension.Space(10f);
                clip = EditorExtension.DisplayCustomField("Clip", false, clip);

                EditorExtension.Space(10f);

                EditorExtension.DrawHorizontal(() => {
                    EditorExtension.DisplayBoldLabel("Audio Source Name");
                    audioSourceReferenceName = EditorGUILayout.TextField(audioSourceReferenceName).ToLower();
                });
            }
#endif
        }

        [System.Serializable]
        public class CharacterSubtitleData : SubtitleData
        {
            [SerializeField] private string characterName;
            public string CharacterName
            {
                get
                {
                    return string.IsNullOrEmpty(characterName) ? "???" : characterName;
                }
            }

            public CharacterSubtitleData() : base() {
                this.characterName = string.Empty;
            }

            public CharacterSubtitleData(string context) : base(context)
            {
                this.characterName = string.Empty;
            }

            public CharacterSubtitleData(string characterName, string context) : base(context)
            {
                this.characterName = characterName;
            }

            public CharacterSubtitleData(string context, GameTime startTime) : base(context, startTime)
            {
                this.characterName = string.Empty;
            }

            public CharacterSubtitleData(string characterName, string context, GameTime startTime) : base(context, startTime)
            {
                this.characterName = characterName;
            }

            public CharacterSubtitleData(string context, GameTime startTime, GameTime endTime) : base(context, startTime, endTime)
            {
                this.characterName = string.Empty;
            }

            public CharacterSubtitleData(string characterName, string context, GameTime startTime, GameTime endTime) : base(context, startTime, endTime)
            {
                this.characterName = characterName;
            }

            public CharacterSubtitleData(string characterName, string context, string anchorName) : base(context, anchorName)
            {
                this.characterName = characterName;
            }

            public CharacterSubtitleData(string characterName, string context, string anchorName, GameTime startTime) : base(context, anchorName, startTime)
            {
                this.characterName = characterName;
            }

            public CharacterSubtitleData(string characterName, string context, string anchorName, GameTime startTime, GameTime endTime) : base(context, anchorName, startTime, endTime)
            {
                this.characterName = characterName;
            }

            protected override void Update(SubtitleDisplayer displayer, SubtitleManager manager, float progress)
            {
                displayer?.DisplayCharacterName(characterName, manager);
                base.Update(displayer, manager, progress);
            }

#if UNITY_EDITOR
            protected override void DrawBaseSettings()
            {
                base.DrawBaseSettings();

                EditorExtension.Space(10f);
                EditorExtension.DrawHorizontal(() => {
                    EditorExtension.DisplayBoldLabel("Character Name");
                    characterName = EditorGUILayout.TextField(characterName);
                });
            }
#endif
        }

        [System.Serializable]
        public sealed class AudibleCharacterSubtitleData : CharacterSubtitleData
        {
            [Space]
            [SerializeField] private AudioClip clip;

            [Space]
            [SerializeField] private string audioSourceReferenceName;

            private static readonly Dictionary<AudibleCharacterSubtitleData, AudioSource> dataSources = new Dictionary<AudibleCharacterSubtitleData, AudioSource>();

            public AudibleCharacterSubtitleData() {
                this.clip = null;
                this.audioSourceReferenceName = string.Empty;
            }

            public AudibleCharacterSubtitleData(string context) : base(context)
            {
                this.clip = null;
                audioSourceReferenceName = string.Empty;
            }

            public AudibleCharacterSubtitleData(string context, AudioClip clip) : base(context)
            {
                this.clip = clip;
                audioSourceReferenceName = string.Empty;
            }

            public AudibleCharacterSubtitleData(string characterName, string context) : base(characterName, context)
            {
                this.clip = null;
                audioSourceReferenceName = string.Empty;
            }

            public AudibleCharacterSubtitleData(string characterName, string context, AudioClip clip) : base(characterName, context)
            {
                this.clip = clip;
                audioSourceReferenceName = string.Empty;
            }


            public AudibleCharacterSubtitleData(string context, GameTime startTime) : base(context, startTime)
            {
                this.clip = null;
                audioSourceReferenceName = string.Empty;
            }

            public AudibleCharacterSubtitleData(string context, AudioClip clip, GameTime startTime) : base(context, startTime)
            {
                this.clip = clip;
                audioSourceReferenceName = string.Empty;
            }

            public AudibleCharacterSubtitleData(string characterName, string context, GameTime startTime) : base(characterName, context, startTime)
            {
                this.clip = null;
                audioSourceReferenceName = string.Empty;
            }

            public AudibleCharacterSubtitleData(string characterName, string context, AudioClip clip, GameTime startTime) : base(characterName, context, startTime)
            {
                this.clip = clip;
                audioSourceReferenceName = string.Empty;
            }

            public AudibleCharacterSubtitleData(string context, GameTime startTime, GameTime endTime) : base(context, startTime, endTime)
            {
                this.clip = null;
                audioSourceReferenceName = string.Empty;
            }

            public AudibleCharacterSubtitleData(string context, AudioClip clip, GameTime startTime, GameTime endTime) : base(context, startTime, endTime)
            {
                this.clip = clip;
                audioSourceReferenceName = string.Empty;
            }

            public AudibleCharacterSubtitleData(string characterName, string context, GameTime startTime, GameTime endTime) : base(characterName, context, startTime, endTime)
            {
                this.clip = null;
                audioSourceReferenceName = string.Empty;
            }

            public AudibleCharacterSubtitleData(string characterName, string context, AudioClip clip, GameTime startTime, GameTime endTime) : base(characterName, context, startTime, endTime)
            {
                this.clip = clip;
                audioSourceReferenceName = string.Empty;
            }

            protected sealed override IEnumerator Update(float timeElapsed, SubtitleDisplayer displayer, SubtitleUpdater updater, CancellationToken token) {
                AudioSource source = AudioSourceReference.GetSource(audioSourceReferenceName);

                if (source != null) {
                    source.clip = this.clip;
                    source?.Play();

                    if (this.clip != null) source.time = timeElapsed;
                }

                yield return CoroutineManager.Start(base.Update(timeElapsed, displayer, updater, token));
                source?.Stop();
            }


#if UNITY_EDITOR
            protected sealed override void DrawBaseSettings()
            {
                base.DrawBaseSettings();

                EditorExtension.Space(10f);
                clip = EditorExtension.DisplayCustomField("Clip", false, clip);

                EditorExtension.Space(10f);

                EditorExtension.DrawHorizontal(() => {
                    EditorExtension.DisplayBoldLabel("Audio Source Name");
                    audioSourceReferenceName = EditorGUILayout.TextField(audioSourceReferenceName).ToLower();
                });
            }
#endif
        }
    }
}
