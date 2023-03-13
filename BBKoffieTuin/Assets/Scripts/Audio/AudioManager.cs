using System;
using Route;
using Toolbox.MethodExtensions;
using Toolbox.Utilities;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : MonoSingleton<AudioManager>
    {
        private AudioSource _audioSource;
        private RouteHandler _routeHandler;

        public UnityEvent onClipChanged = new UnityEvent();
        public UnityEvent<AudioClip> onClipComplete = new();
        
        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _routeHandler = RouteHandler.Instance;
        }

        private void Start()
        {
            _routeHandler.onPointReached.AddListener(HandlePointReached);
        }

        private void HandlePointReached(RoutePoint point, int index)
        {
            if (point.AudioPaths.IsEmpty()) return;
            
            int audioIndex = Random.Range(0, point.AudioPaths.Count);

            var clip = Resources.Load<AudioClip>(point.AudioPaths[audioIndex]);
            if (clip == null)
            {
                Debug.LogWarning("Unable to load audio clip from: " + point.AudioPaths[0]);
                return;
            }
            
            Play(clip);
        }

        private void Update()
        {
            if (_audioSource.clip == null) return;
            if (!_audioSource.isPlaying && Math.Abs(_audioSource.time - _audioSource.clip.length) < 0.01)
            {
                onClipComplete?.Invoke(_audioSource.clip);
            }
        }

        public void Play(AudioClip clip)
        {
            _audioSource.Stop();
            _audioSource.clip = clip;
            onClipChanged.Invoke();

            _audioSource.time = 0;
            _audioSource.Play();
        }
        
        public AudioSource AudioSource => _audioSource;

    }
}
