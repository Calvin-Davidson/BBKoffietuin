using System;
using UnityEngine;
using UnityEngine.Events;

namespace Audio
{
    public class AudioForcer : MonoBehaviour
    {
        public AudioClip clip;
        public UnityEvent onComplete = new();
        public bool playOnAwake;

        private void Awake()
        {
            AudioManager.Instance.onClipComplete.AddListener(HandleClipComplete);
            if (playOnAwake) Play();
        }

        public void Play()
        {
            AudioManager.Instance.Play(clip);
        }

        private void HandleClipComplete(AudioClip audioClip)
        {
            if (audioClip == clip) onComplete?.Invoke();
        }
    }
}
