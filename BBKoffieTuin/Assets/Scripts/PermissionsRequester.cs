using UnityEngine;
using UnityEngine.Android;
using UnityEngine.Events;

public class PermissionsRequester : MonoBehaviour
{
    public UnityEvent onPermissionGranted = new();
    public UnityEvent onPermissionDenied = new();
    public UnityEvent onPermissionDeniedDontAskAgain = new();
    public UnityEvent onPermissionShouldAsk = new();
    public UnityEvent onPermissionAcceptedBefore = new();

    public void Request(string permission)
    {
        if (Permission.HasUserAuthorizedPermission(permission))
        {
            onPermissionAcceptedBefore?.Invoke();
            return;
        }
        PermissionCallbacks callbacks = new PermissionCallbacks();
        callbacks.PermissionGranted += (_) => onPermissionGranted?.Invoke();
        callbacks.PermissionDenied += (_) => onPermissionDenied?.Invoke();
        callbacks.PermissionDeniedAndDontAskAgain += (_) => onPermissionDeniedDontAskAgain?.Invoke();
        Permission.RequestUserPermission(permission, callbacks);
    }

    public void RequestGalleryPermissions(NativeGallery.PermissionType permission, NativeGallery.MediaType type)
    {
        var result = NativeGallery.RequestPermission(permission, type);

        switch (result)
        {
            case NativeGallery.Permission.Denied:
                onPermissionDenied?.Invoke();
                break;
            case NativeGallery.Permission.Granted:
                onPermissionGranted?.Invoke();
                break;
            case NativeGallery.Permission.ShouldAsk:
                onPermissionShouldAsk?.Invoke();
                break;
        }
    }

    public void RequestCameraPermission()
    {
        Request(Permission.Camera);
    }
    
    public void RequestMicrophonePermission()
    {
        Request(Permission.Microphone);
    }

    public void RequestLocationPermission()
    {
        Request(Permission.FineLocation);
    }

    public void RequestGalleryImageWrite()
    {
        RequestGalleryPermissions(NativeGallery.PermissionType.Write, NativeGallery.MediaType.Image);        
    }
    
    public void RequestGalleryImageRead()
    {
        RequestGalleryPermissions(NativeGallery.PermissionType.Read, NativeGallery.MediaType.Image);
    }
}
