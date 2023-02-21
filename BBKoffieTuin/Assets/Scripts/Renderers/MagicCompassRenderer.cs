using System;
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
            Quaternion currentRot = northPointer.transform.rotation;
            currentRot.z = newRotation;

            northPointer.transform.rotation = currentRot;
        }
        
        private void RenderMagicPoint(float newRotation)
        {
            Quaternion currentRot = magicPointer.transform.rotation;
            currentRot.z = newRotation;

            magicPointer.transform.rotation = currentRot;
        }
    }
}
