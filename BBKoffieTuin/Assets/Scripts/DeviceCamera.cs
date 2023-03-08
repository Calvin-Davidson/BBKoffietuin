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

    private void Awake()
    {
        StartWebCam();
    }

    public void StartWebCam()
    {
        _webCamTexture = new WebCamTexture(WebCamTexture.devices.FirstOrDefault(device => device.isFrontFacing).name);
        _webCamTexture.Play();
        displayImage.texture = _webCamTexture;
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
