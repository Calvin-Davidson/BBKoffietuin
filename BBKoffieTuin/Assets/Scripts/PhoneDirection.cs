using System;
using TMPro;
using UnityEngine;

public class PhoneDirection : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI directionDebug;
    [SerializeField] private TextMeshProUGUI accuracyDebug;
    [SerializeField] private TextMeshProUGUI nextPointDebug;
    [SerializeField] private TextMeshProUGUI currentLocationDebug;
    [SerializeField] private TextMeshProUGUI nextLocationDebug;
    [SerializeField] private TextMeshProUGUI distanceDebug;
    
    [SerializeField] private RectTransform windrose;
    [SerializeField] private RectTransform pointer;
    private Vector2 _currentPos;
    private Vector2 _nextPos = new Vector2(4.856711f, 52.390652f);

    private void Awake()
    {
        Input.compass.enabled = true;
    }

    void Update()
    {
        float latitude = Input.location.lastData.latitude;
        float longitude = Input.location.lastData.longitude;
        
        float northHeadingDegrees = Input.compass.magneticHeading;
        float headingAccuracy = Input.compass.headingAccuracy;

        float distance = (_nextPos - _currentPos).magnitude;

        _currentPos = new Vector2(longitude, latitude);

        currentLocationDebug.text = _currentPos.ToString();
        nextLocationDebug.text = _nextPos.ToString();
        distanceDebug.text = (_nextPos - _currentPos).magnitude.ToString();
        
        directionDebug.text = northHeadingDegrees.ToString();
        accuracyDebug.text = headingAccuracy.ToString();

        Vector2 lookDirection = (_nextPos - _currentPos).normalized;

        float directionInDegrees = Mathf.Atan2(lookDirection.y, lookDirection.x);

        directionInDegrees = directionInDegrees * 180 / Mathf.PI;

        directionInDegrees = (directionInDegrees - 90);

        nextPointDebug.text = directionInDegrees.ToString();

        float nextPointDegrees = northHeadingDegrees + directionInDegrees;

        Quaternion windroseTarget = Quaternion.Euler(new Vector3(0, 0, northHeadingDegrees));
        Quaternion pointerTarget = Quaternion.Euler(new Vector3(0, 0, nextPointDegrees));
        
        float windroseRotationSpeed = (windroseTarget.eulerAngles - windrose.eulerAngles).magnitude;
        float pointerRotationSpeed = (pointerTarget.eulerAngles - pointer.eulerAngles).magnitude;
        windrose.rotation = Quaternion.RotateTowards(windrose.rotation, windroseTarget, Time.deltaTime * windroseRotationSpeed);
        pointer.rotation = Quaternion.RotateTowards(pointer.rotation, pointerTarget, Time.deltaTime * pointerRotationSpeed);
    }
}
