using System.Collections;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.Events;

public class GpsServiceStarter : MonoBehaviour
{
    [SerializeField] private int maxWaitInSeconds = 15;

    public UnityEvent onLocationServicesStarted = new UnityEvent();
    public bool GpsServiceEnabled { get; private set; } = false;

    private bool _hasFineLocationPermission = false;
    private PermissionCallbacks _permissionCallbacks;
    
    IEnumerator Start()
    {
        //handle permission
        _hasFineLocationPermission = Permission.HasUserAuthorizedPermission(Permission.FineLocation);
        
        //WE HAVE PERMISSION SO WE CAN START THE SERVICE
        if (_hasFineLocationPermission)
        {
            StartCoroutine(StartLocationServices());
            yield break;
        }
        
        //WE DON'T HAVE PERMISSION SO WE REQUEST IT AND START SERVICES ON GRANTED.
        _permissionCallbacks.PermissionGranted += s => { StartCoroutine(StartLocationServices()); };

        _permissionCallbacks.PermissionDenied += s => { };

        _permissionCallbacks.PermissionDeniedAndDontAskAgain += s => { };
            
        Permission.RequestUserPermission(Permission.FineLocation, _permissionCallbacks);
    }

    public IEnumerator StartLocationServices()
    {
        // First, check if user has location service enabled
        if (!Input.location.isEnabledByUser) {
            Debug.LogFormat("Android and Location not enabled");
            yield break;
        }

        // Start service before querying location
        Input.location.Start();

        // Wait until service initializes
        while (Input.location.status == LocationServiceStatus.Initializing && maxWaitInSeconds > 0)
        {
            yield return new WaitForSecondsRealtime(1);
            maxWaitInSeconds--;
        }

        // Service didn't initialize in 15 seconds
        if (maxWaitInSeconds < 1)
        {
            Debug.LogFormat("Timed out");
            yield break;
        }

        // Connection has failed
        if (Input.location.status != LocationServiceStatus.Running)
        {
            Debug.LogFormat("Unable to determine device location. Failed with status {0}", Input.location.status);
            yield break;
        }

        Debug.LogFormat("Location service live. status {0}", Input.location.status);
        // Access granted and location value could be retrieved
        Debug.LogFormat("Location: "
                        + Input.location.lastData.latitude + " "
                        + Input.location.lastData.longitude + " "
                        + Input.location.lastData.altitude + " "
                        + Input.location.lastData.horizontalAccuracy + " "
                        + Input.location.lastData.timestamp);

        float latitude = Input.location.lastData.latitude;
        float longitude = Input.location.lastData.longitude;
        // TODO success do something with location

        GpsServiceEnabled = true;
        onLocationServicesStarted.Invoke();

        // Stop service if there is no need to query location updates continuously
        // Input.location.Stop();
    }
}