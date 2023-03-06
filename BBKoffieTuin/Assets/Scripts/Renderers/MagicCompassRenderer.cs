using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Renderers
{
    public class MagicCompassRenderer : MonoBehaviour
    {
        [SerializeField] private RectTransform northPointerTransform;
        [SerializeField] private RectTransform magicPointerTransform;
        
        private void Start()
        {
            PhoneDirection.Instance.onNortherPointChange.AddListener(RenderNortherPoint);
            PhoneDirection.Instance.onTargetPointChange.AddListener(RenderMagicPoint);
        }

        private void RenderNortherPoint(float newRotation)
        {
            northPointerTransform.rotation = Quaternion.Euler(new Vector3(0,0,newRotation));
        }
        
        private void RenderMagicPoint(float newRotation)
        {
            magicPointerTransform.rotation = Quaternion.Euler(new Vector3(0,0,newRotation));;
        }
    }
}
