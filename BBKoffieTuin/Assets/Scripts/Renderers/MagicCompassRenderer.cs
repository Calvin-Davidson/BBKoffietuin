using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Renderers
{
    public class MagicCompassRenderer : MonoBehaviour
    {
        [SerializeField] private Image northPointer;
        [SerializeField] private Image magicPointer;
        [SerializeField] private PhoneDirection phoneDirection;
        
        private void Start()
        {
            phoneDirection.onNortherPointChange.AddListener(RenderNortherPoint);
            phoneDirection.onTargetPointChange.AddListener(RenderMagicPoint);
        }

        private void RenderNortherPoint(float newRotation)
        {
            northPointer.rectTransform.rotation = Quaternion.Euler(new Vector3(0,0,newRotation));
        }
        
        private void RenderMagicPoint(float newRotation)
        {
            magicPointer.rectTransform.rotation = Quaternion.Euler(new Vector3(0,0,newRotation));;
        }
    }
}
