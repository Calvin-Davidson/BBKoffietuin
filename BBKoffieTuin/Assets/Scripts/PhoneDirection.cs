using System;
using TMPro;
using UnityEngine;

public class PhoneDirection : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI directionDebug;
    [SerializeField] private TextMeshProUGUI accuracyDebug;
    [SerializeField] private RectTransform windrose;

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

        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, headingDegrees));
        
        float rotationSpeed = (targetRotation.eulerAngles - windrose.eulerAngles).magnitude;
        windrose.rotation = Quaternion.RotateTowards(windrose.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }
}
