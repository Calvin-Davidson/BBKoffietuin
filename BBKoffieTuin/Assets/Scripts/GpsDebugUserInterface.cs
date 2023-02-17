using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class GpsDebugUserInterface : MonoBehaviour
{
    [SerializeField] private GpsService gpsService;

    [SerializeField] private TMP_Text latitudeText;
    [SerializeField] private TMP_Text longitudeText;
    [SerializeField] private TMP_Text altitudeText;
    [SerializeField] private TMP_Text horizontalAccuracyText;
    [SerializeField] private TMP_Text timestampText;

    private void Update()
    {
        if (!GpsService.Instance.GpsServiceEnabled) return;

        latitudeText.text = "latitude: " + Input.location.lastData.latitude;
        longitudeText.text = "longitude: " + Input.location.lastData.longitude;
        altitudeText.text = "altitude: " + Input.location.lastData.altitude;
        horizontalAccuracyText.text = "horizontalAccuracy: " + Input.location.lastData.horizontalAccuracy;
        timestampText.text = "timestamp: " + Input.location.lastData.timestamp;
    }
}