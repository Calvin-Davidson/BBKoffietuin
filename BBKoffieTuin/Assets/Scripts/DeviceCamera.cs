using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using File = System.IO.File;

public class DeviceCamera : MonoBehaviour
{
    [SerializeField, Tooltip("Used to display the camera's texture")] private RawImage displayImage;

    private WebCamTexture _webCamTexture;

    public UnityEvent onPictureComplete = new();
    
    private void Start()
    {
        StartWebCam();
    }

    public void StartWebCam()
    {
        string frontCamName = WebCamTexture.devices.FirstOrDefault(device => device.isFrontFacing).name;
        _webCamTexture = new WebCamTexture(frontCamName);
        _webCamTexture.Play();

        Canvas canvas = displayImage.GetComponentInParent<Canvas>();
        float scale = Math.Max(canvas.pixelRect.width / _webCamTexture.width, canvas.pixelRect.height / _webCamTexture.height);

        displayImage.texture = _webCamTexture;
        displayImage.rectTransform.sizeDelta = new Vector2(_webCamTexture.width * scale, _webCamTexture.height * scale);
    }

    public void TakePicture()
    {
        StartCoroutine(TakePictureCoroutine());
    }


    private IEnumerator TakePictureCoroutine()
    {
        var perm = NativeGallery.CheckPermission(NativeGallery.PermissionType.Write, NativeGallery.MediaType.Image);
        if (perm == NativeGallery.Permission.Denied)
        {
            gameObject.SetActive(false);
            yield break;
        }
        
       yield return new WaitForEndOfFrame();
        
        Texture2D photo = new Texture2D(_webCamTexture.width, _webCamTexture.height);
        photo.SetPixels(_webCamTexture.GetPixels());
        photo.Apply();
        
        string pictureName = "BBStories" + System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".png";
        //Encode to a PNG
        byte[] bytes = photo.EncodeToPNG();
        //Write out the PNG. Of course you have to substitute your_path for something sensible

        NativeGallery.SaveImageToGallery(bytes, "BBStories", pictureName, (success, path) =>
        {
            onPictureComplete?.Invoke();
        });
    }
}
