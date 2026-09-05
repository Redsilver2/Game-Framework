using RedSilver2.Framework.StateMachines.Extensions;
using RedSilver2.Framework.StateMachines.States;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.Events
{
    [System.Serializable]
    public sealed partial class JumpAudio : MovementAudio
    {
        [Space]
        [SerializeField, HideInInspector] private List<AudioClip> clips;
        private const string EVENT_NAME = "Jump Audio";

        private JumpAudio(JumpState state) : base(state) {
            clips = new List<AudioClip>();
        }

        public void SetClips(List<AudioClip> clips) { this.clips = clips; }
        protected sealed override void Disable(MovementState state) {
            state?.RemoveOnEnteredListener(OnEntered);
        }

        protected sealed override void Enable(MovementState state) {
            state?.AddOnEnteredListener(OnEntered);
        }

        protected sealed override string GetName() {
            return EVENT_NAME;
        }

        private void OnEntered() {
            AudioSource source = Source;

            if(source != null && clips != null) {
                AudioClip clip = clips.Count <= 0 ? null : clips[Random.Range(0, clips.Count)];
                if(clip != null) { source?.PlayOneShot(clip);}
            }
        }
    }

    public sealed partial class JumpAudio : MovementAudio {
#if UNITY_EDITOR
        [SerializeField, HideInInspector] private AudioClip selectedClip;      
        [SerializeField, HideInInspector] private bool showAddClip;
        [SerializeField, HideInInspector] private bool showRemoveClip;

        protected override void DrawInspectorSettings(StateMachine.InspectorVisualizer visualizer)
        {
            base.DrawInspectorSettings(visualizer);
            if (visualizer == null) return;

            if (EditorExtension.DisplayFoldout("Add Clip", ref showAddClip, visualizer.FoldoutColor)) {
                EditorExtension.DrawVerticalHelpBox(() =>
                {
                    EditorExtension.IncrementIndent();

                    if (clips == null) { EditorExtension.DisplayHelpBoxError("Clips is null"); }
                    else
                    {
                        selectedClip = EditorExtension.DisplayCustomField("Selected Clip", false, selectedClip);

                        if (selectedClip != null)
                        {
                            if (clips.Contains(selectedClip))
                            {
                                EditorExtension.DisplayHelpBoxWarning("Clip is already added");
                            }
                            else
                            {
                                EditorExtension.DisplayButton("Add Clip",  () =>
                                {
                                    clips?.Add(selectedClip);
                                    selectedClip = null;
                                });
                            }
                        }
                        else { EditorExtension.DisplayHelpBoxError("Clip is null so it can't be added"); }
                    }

                    EditorExtension.DecrementIndent();
                });
            }


            if (EditorExtension.DisplayFoldout("Remove Clip", ref showRemoveClip, visualizer.FoldoutColor))
            {
              
                EditorExtension.IncrementIndent();
                if (clips == null) { EditorExtension.DisplayHelpBoxError("Clips is null"); }
                else if (clips.Count > 0) {
                    for (int i = 0; i < clips.Count; i++) {
                        if (clips[i] == null) continue;
                        UnityEditor.EditorGUILayout.BeginHorizontal();
                        EditorExtension.DisplayBoldLabel(clips[i].name);

                        EditorExtension.DisplayButton("Remove", () => { clips[i] = null; });
                        UnityEditor.EditorGUILayout.EndHorizontal();
                    }

                    this.clips = clips.Where(x => x != null).ToList();
                }
                else { EditorExtension.DisplayHelpBoxError("No clips to remove"); }

                EditorExtension.DecrementIndent();
            }       
        }

        public static void DrawInspector(JumpState state, ref bool showEvent, StateMachine.InspectorVisualizer visualizer) {
            DrawInspector(state, EVENT_NAME, ref showEvent, visualizer,
              () => {
                  EditorExtension.DisplayButton($"Add {EVENT_NAME}", () => { state?.AddEvent(EVENT_NAME, new JumpAudio(state)); });
              },
              () => {
                  EditorExtension.DisplayButton($"Remove {EVENT_NAME}", () => { state?.RemoveEvent(EVENT_NAME); });
              });
        }
#endif
    }
}
