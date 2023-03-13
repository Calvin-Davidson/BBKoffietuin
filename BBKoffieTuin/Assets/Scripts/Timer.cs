using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    [SerializeField] private float duration;
    [SerializeField] private bool runOnAwake;
    
    private float _timePast;
    private bool _isRunning;
    
    public UnityEvent<float> onUpdate = new();
    public UnityEvent onTimeElapsed = new();

    public bool IsRunning
    {
        get => _isRunning;
        set
        {
            _timePast = 0;
            _isRunning = value;
        }
    }

    private void Awake()
    {
        if (runOnAwake) IsRunning = true;
    }

    private void Update()
    {
        if (!IsRunning) return;
        _timePast += Time.deltaTime;
        onUpdate?.Invoke(_timePast);

        if (_timePast > duration)
        {
            _isRunning = false;
            onTimeElapsed.Invoke();
        }
    }
}
