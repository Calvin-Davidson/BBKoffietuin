using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class PhoneDirection : MonoBehaviour
{
    [SerializeField] private float rotationTolerance = 1f;
    
    private Vector2 _currentPos;
    private Vector2 _nextPos = new Vector2(4.856711f, 52.390652f);

    private float _lastNorthRotation = 0;
    private float _lastTargetPointRotation = 0;

    public UnityEvent<float> onNortherPointChange = new UnityEvent<float>();
    public UnityEvent<float> onTargetPointChange = new UnityEvent<float>();

    private void Awake()
    {
        Input.compass.enabled = true;
    }

    void Update()
    {
        float latitude = Input.location.lastData.latitude;
        float longitude = Input.location.lastData.longitude;
        
        float northHeadingDegrees = Input.compass.magneticHeading;
        
        _currentPos = new Vector2(longitude, latitude);
        
        Vector2 lookDirection = (_nextPos - _currentPos).normalized;

        float directionInDegrees = Mathf.Atan2(lookDirection.y, lookDirection.x);

        directionInDegrees = directionInDegrees * 180 / Mathf.PI;

        directionInDegrees = (directionInDegrees - 90);
        
        float nextPointDegrees = northHeadingDegrees + directionInDegrees;
        
        if (Math.Abs(nextPointDegrees - _lastTargetPointRotation) > rotationTolerance) onTargetPointChange.Invoke(nextPointDegrees);
        if (Math.Abs(northHeadingDegrees - _lastNorthRotation) > rotationTolerance) onNortherPointChange.Invoke(northHeadingDegrees);

        _lastNorthRotation = northHeadingDegrees;
        _lastTargetPointRotation = nextPointDegrees;
    }
}
