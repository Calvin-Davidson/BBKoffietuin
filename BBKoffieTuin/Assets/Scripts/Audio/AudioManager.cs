using Route;
using Toolbox.MethodExtensions;
using UnityEngine;

namespace Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : MonoBehaviour
    {
        private AudioSource _audioSource;
        private RouteHandler _routeHandler;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _routeHandler = FindObjectOfType<RouteHandler>();
        }

        private void Start()
        {
            _routeHandler.onPointReached.AddListener(HandlePointReached);
        }

        private void HandlePointReached(RoutePoint point, int index)
        {
            if (_audioSource.isPlaying) return;
            if (point.AudioPaths.IsEmpty()) return;

            if (Resources.Load(point.AudioPaths[0]) == null)
            {
                Debug.LogWarning("Unable to load audio clip from: " + point.AudioPaths[0]);
                return;
            }

            Debug.Log(Resources.Load(point.AudioPaths[0], typeof(AudioClip)) as AudioClip);
            _audioSource.clip = Resources.Load(point.AudioPaths[0], typeof(AudioClip)) as AudioClip;
            _audioSource.time = 0;
            _audioSource.Play();
        }
    }
}
