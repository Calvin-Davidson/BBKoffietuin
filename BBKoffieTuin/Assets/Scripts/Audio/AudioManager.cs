using Route;
using Toolbox.MethodExtensions;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : MonoBehaviour
    {
        private AudioSource _audioSource;
        private RouteHandler _routeHandler;

        public UnityEvent onClipChanged = new UnityEvent();
            
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
            
            _audioSource.Stop();
            _audioSource.clip = clip;
            onClipChanged.Invoke();

            _audioSource.time = 0;
            _audioSource.Play();
        }

        public AudioSource AudioSource => _audioSource;

    }
}
