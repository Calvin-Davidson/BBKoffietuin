using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class GpsDebugUserInterface : MonoBehaviour
{
    [SerializeField] private GpsService gpsService;

    public TMP_Text latitudeText;
    public TMP_Text longitudeText;
    public TMP_Text altitudeText;
    public TMP_Text horizontalAccuracyText;
    public TMP_Text timestampText;

    private void Update()
    {
        if (!gpsService.GpsServiceEnabled) return;

        latitudeText.text = "latitude: " + Input.location.lastData.latitude;
        longitudeText.text = "longitude: " + Input.location.lastData.longitude;
        altitudeText.text = "altitude: " + Input.location.lastData.altitude;
        horizontalAccuracyText.text = "horizontalAccuracy: " + Input.location.lastData.horizontalAccuracy;
        timestampText.text = "timestamp: " + Input.location.lastData.timestamp;
    }
}