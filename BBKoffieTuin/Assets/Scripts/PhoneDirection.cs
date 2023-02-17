using System;
using TMPro;
using UnityEngine;

public class PhoneDirection : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI directionDebug;
    [SerializeField] private TextMeshProUGUI accuracyDebug;
    [SerializeField] private TextMeshProUGUI nextPointDebug;
    [SerializeField] private RectTransform windrose;
    private Vector2 _currentPos = new Vector2(4.856711f, 52.390652f);
    private Vector2 _nextPos = new Vector2(4.856711f, 52.390652f);

    private void Awake()
    {
        Input.compass.enabled = true;
    }

    void Update()
    {
        float headingDegrees = Input.compass.magneticHeading;
        float headingAccuracy = Input.compass.headingAccuracy;
        
        directionDebug.text = headingDegrees.ToString();
        accuracyDebug.text = headingAccuracy.ToString();

        Vector2 lookDirection = (_nextPos - _currentPos).normalized;

        float directionInDegrees = Mathf.Atan2(lookDirection.y, lookDirection.x);

        directionInDegrees = directionInDegrees * 180 / Mathf.PI;

        directionInDegrees = (directionInDegrees - 90) * -1;

        nextPointDebug.text = directionInDegrees.ToString();

        directionInDegrees += headingDegrees;
        
        windrose.rotation = Quaternion.Euler(0, 0, directionInDegrees);

        // Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, headingDegrees));
        //
        // float rotationSpeed = (targetRotation.eulerAngles - windrose.eulerAngles).magnitude;
        // windrose.rotation = Quaternion.RotateTowards(windrose.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }
}
