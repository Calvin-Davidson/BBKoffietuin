using System;
using System.Collections.Generic;
using Route;
using Toolbox.Utilities;
using UnityEngine;

namespace Audio
{
    public class RouteCodeAudio : MonoBehaviour
    {
        [SerializeField] private AudioClip codeStartClip;
        [SerializeField] private List<AudioClip> codeClip;

        private RouteCodeHandler _routeCodeHandler;
        private AudioClip _codeToPlay;
        
        private void Awake()
        {
            _routeCodeHandler = FindObjectOfType<RouteCodeHandler>();
            _routeCodeHandler.onCodeChange.AddListener(HandleCodeChange);
            AudioManager.Instance.onClipComplete.AddListener(HandleAudioClipComplete);
        }

        private void HandleCodeChange(char code, int slot)
        {
            AudioManager.Instance.Play(codeStartClip);
            _codeToPlay = codeClip[int.Parse(code.ToString())];
        }

        private void HandleAudioClipComplete(AudioClip clip)
        {
            if (clip == codeStartClip && _codeToPlay != null)
            {
                AudioManager.Instance.Play(_codeToPlay);
                _codeToPlay = null;
            } 
        }
    }
}
