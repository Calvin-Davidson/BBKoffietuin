using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StopwatchRenderer : MonoBehaviour
{
    [SerializeField] private RectTransform needleTransform;


    public void Render(float progress)
    {
        float clampedProgress = progress / 60;
        clampedProgress = Mathf.Clamp01(clampedProgress);
        needleTransform.rotation = Quaternion.Euler(new Vector3(0,0, clampedProgress * 360));
    }
}
