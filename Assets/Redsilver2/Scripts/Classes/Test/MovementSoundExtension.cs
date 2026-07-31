using RedSilver2.Framework.StateMachines.Events;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.StateMachines.Extensions
{
    [RequireComponent(typeof(AudioSource))]

    public class MovementSoundExtension : MovementStateMachineEvent
    {
        [SerializeField] private MovementSoundData[] movementSoundDatas;
        [SerializeField] private float soundTriggerTime;

        private AudioSource source;
        private MovementSoundData currentData;

        private Dictionary<string, MovementSoundData> tagSoundDatas;


        private float currentMoveSoundTriggerTime;

        protected override void Awake()
        {
            base.Awake();

            source = GetComponent<AudioSource>();
            tagSoundDatas = new Dictionary<string, MovementSoundData>();

            foreach(MovementSoundData data in movementSoundDatas) {
                if (data == null || tagSoundDatas.ContainsKey(data.groundTag.ToLower())) return;
                tagSoundDatas?.Add(data.groundTag.ToLower(), data);
            }
        }

        protected override void SetStateMachineEvents(MovementStateMachine stateMachine, bool isAddingEvents)
        {
            if (isAddingEvents) {
                stateMachine?.AddOnUpdateListener(OnUpdate(stateMachine));
                stateMachine?.AddOnGroundTagChangedListener(OnGroundTagChanged);
            }
            else {
                stateMachine?.RemoveOnUpdateListener(OnUpdate(stateMachine));
                stateMachine?.RemoveOnGroundTagChangedListener(OnGroundTagChanged);
            }
        }

        private UnityAction OnUpdate(MovementStateMachine stateMachine) {
            return () =>
            {
                if (stateMachine == null) return;
                else if (stateMachine.IsMoving && stateMachine.IsGrounded) {
                    currentMoveSoundTriggerTime = Mathf.Clamp(Time.deltaTime + currentMoveSoundTriggerTime, 0f, soundTriggerTime);

                    if (currentMoveSoundTriggerTime >= soundTriggerTime) {
                        currentMoveSoundTriggerTime = 0f;
                        TriggerMoveSoundUpdate();
                    }
                }
            };
        }

        private void OnGroundTagChanged(string value) {
            currentData = GetMovementSoundData(value);
            currentMoveSoundTriggerTime = soundTriggerTime;
        }

        private void TriggerMoveSoundUpdate()
        {
            if(source == null || currentData == null) return;
            source.pitch  = Random.Range(0.6f, 0.8f);
            source.volume = Random.Range(0.8f, 1f);

            source.clip = GetRandomAudioClip(currentData.moveAudioClips);
            source.Play();
        }

        private AudioClip GetRandomAudioClip(AudioClip[] clips) {
            if(clips == null || clips.Length == 0) return null;
            return clips[Random.Range(0, clips.Length)];
        }

        private MovementSoundData GetMovementSoundData(string groundTag) {
            if(string.IsNullOrEmpty(groundTag) || tagSoundDatas == null) return null;

            if(tagSoundDatas.ContainsKey(groundTag.ToLower()))
                return tagSoundDatas[groundTag.ToLower()];

            return null;
        }
    }
}
