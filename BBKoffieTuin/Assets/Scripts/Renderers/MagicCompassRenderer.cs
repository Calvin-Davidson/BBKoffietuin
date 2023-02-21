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
        [SerializeField] private TextMeshProUGUI debugText1;
        [SerializeField] private TextMeshProUGUI debugText2;

        private void Start()
        {
            phoneDirection.onNortherPointChange.AddListener(RenderNortherPoint);
            phoneDirection.onTargetPointChange.AddListener(RenderMagicPoint);
        }

        private void RenderNortherPoint(float newRotation)
        {
            northPointer.rectTransform.rotation = Quaternion.Euler(new Vector3(0,0,newRotation));
            debugText1.text = newRotation.ToString();
        }
        
        private void RenderMagicPoint(float newRotation)
        {
            magicPointer.rectTransform.rotation = Quaternion.Euler(new Vector3(0,0,newRotation));;
            debugText2.text = newRotation.ToString();
        }
    }
}
