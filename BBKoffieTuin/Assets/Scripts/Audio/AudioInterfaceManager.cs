using System;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Audio
{
    public class AudioInterfaceManager : MonoBehaviour
    {
        [SerializeField] private AudioManager audioManager;

        [SerializeField] private Slider timeSlider;
        [SerializeField] private TMP_Text timeText;
        [SerializeField] private TMP_Text clipNameText;
        [SerializeField] private Button playToggleButton;

        private void OnEnable()
        {
            if(audioManager == null) return;
            audioManager.onClipChanged.AddListener(UpdateAudioInterface);
        }

        private void OnDisable()
        {
            if(audioManager == null) return;
            audioManager.onClipChanged.RemoveListener(UpdateAudioInterface);
        }

        private void Awake()
        {
            audioManager ??= FindObjectOfType<AudioManager>();
            if(audioManager == null)
            {
                Debug.LogWarning("Audio Manager not found removing Audio Interface Manager");
                Destroy(this);
            }
            
            audioManager.onClipChanged.AddListener(UpdateAudioInterface);
            
            playToggleButton.onClick.AddListener(() =>
            {
                if (audioManager.AudioSource.isPlaying)
                {
                    audioManager.AudioSource.Pause();
                }
                else
                {
                    audioManager.AudioSource.Play();
                }
            });
            
            timeSlider.onValueChanged.AddListener(value =>
            {
                if (audioManager.AudioSource.clip == null) return;
                if (Math.Abs(value - .9999) < .001)
                {
                    audioManager.AudioSource.Pause();      
                }
                else
                {
                    audioManager.AudioSource.Play();
                }
                audioManager.AudioSource.time = (value * audioManager.AudioSource.clip.length);
            });
        }

        private void Update()
        {
            UpdateAudioInterface();
        }

        private void UpdateAudioInterface()
        {
            if (audioManager == null) return;
            if (audioManager.AudioSource.clip == null) return;
            
            AudioSource source = audioManager.AudioSource;
            AudioClip clip = source.clip;

            var timeInSeconds = source.time;
            var totalTimeInSeconds = source.clip.length;
            
            var timeInMinutes = Math.Floor(timeInSeconds / 60);
            var totalTimeInMinutes = Math.Floor(totalTimeInSeconds / 60);
            
            var timeInSecondsLeft = timeInSeconds % 60;
            var totalTimeInSecondsLeft = totalTimeInSeconds % 60;

            timeSlider.value = audioManager.AudioSource.time / clip.length;
            timeText.text = $"{timeInMinutes:00}:{timeInSecondsLeft:00} / {totalTimeInMinutes:00}:{totalTimeInSecondsLeft:00}";
            clipNameText.text = $"AUDIO: {clip.name}";
        }
    }
}