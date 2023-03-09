using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using File = System.IO.File;

public class DeviceCamera : MonoBehaviour
{
    [SerializeField, Tooltip("Used to display the camera's texture")] private RawImage displayImage;

    private WebCamTexture _webCamTexture;
    
    public AspectRatioFitter imageFitter;

    //set it to either FRONT or BACK
    string myCamera = "BACK";

    // Device cameras
    WebCamDevice frontCameraDevice;
    WebCamDevice backCameraDevice;
    WebCamDevice activeCameraDevice;

    WebCamTexture frontCameraTexture;
    WebCamTexture backCameraTexture;
    WebCamTexture activeCameraTexture;

    // Image rotation
    Vector3 rotationVector = new Vector3(0f, 0f, 0f);

    // Image uvRect
    Rect defaultRect = new Rect(0f, 0f, 1f, 1f);
    Rect fixedRect = new Rect(0f, 1f, 1f, -1f);

    // Image Parent's scale
    Vector3 defaultScale = new Vector3(1f, 1f, 1f);
    Vector3 fixedScale = new Vector3(-1f, 1f, 1f);
    

    private void Start()
    {
        StartWebCam();
    }

    public void StartWebCam()
    {
        string frontCamName = WebCamTexture.devices.FirstOrDefault(device => device.isFrontFacing).name;
        _webCamTexture = new WebCamTexture(frontCamName);
        _webCamTexture.Play();

        float scale = Math.Min(1920 / _webCamTexture.width, 1080 / _webCamTexture.height);
        
        
        displayImage.texture = _webCamTexture;
        displayImage.rectTransform.sizeDelta = new Vector2(_webCamTexture.width * scale, _webCamTexture.height * scale);
    }

    public void TakePicture()
    {
        StartCoroutine(TakePictureCoroutine());
    }


    private IEnumerator TakePictureCoroutine()
    {
        yield return new WaitForEndOfFrame();
        
        Texture2D photo = new Texture2D(_webCamTexture.width, _webCamTexture.height);
        photo.SetPixels(_webCamTexture.GetPixels());
        photo.Apply();
        
        string pictureName = "BBStories" + System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".png";

        //Encode to a PNG
        byte[] bytes = photo.EncodeToPNG();
        //Write out the PNG. Of course you have to substitute your_path for something sensible
        File.WriteAllBytes(Application.persistentDataPath + $"/{pictureName}.png", bytes);
        
        gameObject.SetActive(false);
    }
}
