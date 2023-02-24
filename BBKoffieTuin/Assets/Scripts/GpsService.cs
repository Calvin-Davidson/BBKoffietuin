using System;
using System.Collections;
using Toolbox.Utilities;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.Events;

public class GpsService : MonoSingleton<GpsService>
{
    //variable declaration
    [SerializeField] private int maxWaitInSeconds = 15;

    private bool _hasFineLocationPermission = false;
    private PermissionCallbacks _permissionCallbacks;
    private readonly float _desiredAccuracyInMeters = 1f;
    private readonly float _updateDistanceInMeters = 1f;

    //events
    public UnityEvent onLocationServicesStarted = new UnityEvent();

    //getters and setters
    public bool GpsServiceEnabled { get; private set; } = false;


    public override void Awake()
    {
        base.Awake();
        _hasFineLocationPermission = Permission.HasUserAuthorizedPermission(Permission.FineLocation);
    }

    
    public void TryStartingLocationServices(Action startedCallback = null, Action noPermissionCallback = null,  Action errorCallback = null)
    {
        RequestLocationPermission(() =>
        {
            StartLocationServices(startedCallback, errorCallback);
        }, () =>
        {
            noPermissionCallback?.Invoke();
            errorCallback?.Invoke();
        });
    }


    private void StartLocationServices(Action startedCallback = null, Action errorCallback = null)
    {
        StartCoroutine(StartLocationServicesEnumerator(startedCallback, errorCallback));
    }
    
    /// <summary>
    /// Makes sure that the user has given permission to use the GPS 
    /// </summary>
    /// <param name="onPermissionConfirmed"></param>
    /// <param name="onPermissionDenied"></param>
    public void RequestLocationPermission(Action onPermissionConfirmed, Action onPermissionDenied)
    {
        //CHECK IF WE ALREADY HAVE PERMISSION
        _hasFineLocationPermission = Permission.HasUserAuthorizedPermission(Permission.FineLocation);
        if (_hasFineLocationPermission)
        {
            onPermissionConfirmed?.Invoke();
            return;
        }

        //WE DON'T HAVE PERMISSION SO WE REQUEST IT AND START SERVICES ON GRANTED.
        _permissionCallbacks = new PermissionCallbacks();

        _permissionCallbacks.PermissionGranted += s => { onPermissionConfirmed.Invoke(); };
        _permissionCallbacks.PermissionDenied += s => { onPermissionDenied.Invoke(); };
        _permissionCallbacks.PermissionDeniedAndDontAskAgain += s => { onPermissionDenied.Invoke(); };

        Permission.RequestUserPermission(Permission.FineLocation, _permissionCallbacks);
    }

    private IEnumerator StartLocationServicesEnumerator(Action startedCallback = null, Action errorCallback = null)
    {
        // First, check if user has location service enabled
        if (!Input.location.isEnabledByUser)
        {
            errorCallback?.Invoke();
            Debug.LogFormat("Android and Location not enabled");
            yield break;
        }

        // Start service before querying location
        Input.location.Start(_desiredAccuracyInMeters, _updateDistanceInMeters);

        // Wait until service initializes
        while (Input.location.status == LocationServiceStatus.Initializing && maxWaitInSeconds > 0)
        {
            yield return new WaitForSecondsRealtime(1);
            maxWaitInSeconds--;
        }

        // Service didn't initialize in 15 seconds
        if (maxWaitInSeconds < 1)
        {
            errorCallback?.Invoke();
            Debug.LogFormat("Timed out");
            yield break;
        }

        // Connection has failed
        if (Input.location.status != LocationServiceStatus.Running)
        {
            errorCallback?.Invoke();
            Debug.LogFormat("Unable to determine device location. Failed with status {0}", Input.location.status);
            yield break;
        }

        GpsServiceEnabled = true;
        onLocationServicesStarted.Invoke();
        startedCallback?.Invoke();
    }

    public void StopLocationServices()
    {
        Input.location.Stop();
        GpsServiceEnabled = false;
    }
}