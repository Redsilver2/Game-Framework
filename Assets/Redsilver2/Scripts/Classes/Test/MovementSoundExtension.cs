using RedSilver2.Framework.StateMachines.Controllers;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.Extensions
{
    [RequireComponent(typeof(AudioSource))]

    public class MovementSoundExtension : MovementStateExtension
    {
        [SerializeField] private MovementSoundData[] movementSoundDatas;
        [SerializeField] private float soundTriggerTime;

        private AudioSource source;
        private MovementSoundData currentData;

        private Dictionary<string, MovementSoundData> tagSoundDatas;


        private float currentMoveSoundTriggerTime;

        protected sealed override void Start() {
            source = GetComponent<AudioSource>();
            tagSoundDatas = new Dictionary<string, MovementSoundData>();

            eventHandler?.AddOnUpdateListener(OnUpdate);
            eventHandler?.AddOnGroundTagChangedListener(OnGroundTagChanged);

            foreach (MovementSoundData data in movementSoundDatas) {
                if (data == null || tagSoundDatas.ContainsKey(data.groundTag.ToLower())) return;
                tagSoundDatas?.Add(data.groundTag.ToLower(), data);
            }

        }

        private void OnUpdate() {
            if (eventHandler == null) return;

            if (eventHandler.IsMoving() && eventHandler.IsGrounded()) {
                currentMoveSoundTriggerTime = Mathf.Clamp(Time.deltaTime + currentMoveSoundTriggerTime, 0f, soundTriggerTime);

                if(currentMoveSoundTriggerTime >= soundTriggerTime) {
                    currentMoveSoundTriggerTime = 0f;
                    TriggerMoveSoundUpdate();
                }
            }
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

        protected override void OnDisable() {
            eventHandler?.RemoveOnUpdateListener(OnUpdate);
            eventHandler?.RemoveOnGroundTagChangedListener(OnGroundTagChanged);
        }

        protected override void OnEnable() {
            eventHandler?.AddOnUpdateListener(OnUpdate);
            eventHandler?.AddOnGroundTagChangedListener(OnGroundTagChanged);
        }
    }
}
