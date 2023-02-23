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
        
        private void Start()
        {
            PhoneDirection.Instance.onNortherPointChange.AddListener(RenderNortherPoint);
            PhoneDirection.Instance.onTargetPointChange.AddListener(RenderMagicPoint);
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
