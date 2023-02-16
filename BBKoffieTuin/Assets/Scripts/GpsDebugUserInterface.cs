using TMPro;
using UnityEngine;

public class GpsDebugUserInterface : MonoBehaviour
{
    [SerializeField] private GpsServiceStarter gpsServiceStarter;

    public TMP_Text latitudeText;
    public TMP_Text longitudeText;
    public TMP_Text altitudeText;
    public TMP_Text horizontalAccuracyText;
    public TMP_Text timestampText;

    private void Update()
    {
        if (!gpsServiceStarter.GpsServiceEnabled) return;

        latitudeText.text = "latitude: " + Input.location.lastData.latitude;
        latitudeText.text = "longitude: " + Input.location.lastData.longitude;
        latitudeText.text = "altitude: " + Input.location.lastData.altitude;
        latitudeText.text = "horizontalAccuracy: " + Input.location.lastData.horizontalAccuracy;
        latitudeText.text = "timestamp: " + Input.location.lastData.timestamp;
    }
}