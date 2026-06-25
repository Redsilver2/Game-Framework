using UnityEngine;

namespace RedSilver2.Framework.Dialogs
{
    public interface IAudibleSubtitle 
    {
        void Play();
        void Play(float time);
        void Stop();
        void SetAudioSource(AudioSource source);
    }
}
