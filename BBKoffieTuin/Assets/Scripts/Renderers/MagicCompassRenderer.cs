using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Renderers
{
    public class MagicCompassRenderer : MonoBehaviour
    {
        [SerializeField] private RectTransform northPointerTransform;
        [SerializeField] private RectTransform magicPointerTransform;
        
        private readonly List<float> _northerPoints = new();
        private readonly List<float> _magicPoints = new();
        
        private const int AverageAccuracy = 5;
        
        private void Start()
        {
            PhoneDirection.Instance.onNortherPointChange.AddListener(RenderNortherPoint);
            PhoneDirection.Instance.onTargetPointChange.AddListener(RenderMagicPoint);
        }
        

        private void RenderNortherPoint(float newRotation)
        {
            RenderPointer(northPointerTransform, newRotation, _northerPoints);
        }
        
        private void RenderMagicPoint(float newRotation)
        {
            RenderPointer(magicPointerTransform, newRotation, _magicPoints);
        }

        private void RenderPointer(RectTransform pointerTransform, float newRotation, List<float> pointsContainer)
        {
            float previousRotationAverage = pointsContainer.Count > 1 ? pointsContainer.Sum() / pointsContainer.Count : 0;

            if (previousRotationAverage - 180 > newRotation) newRotation += 360;
            if (previousRotationAverage + 180 < newRotation) newRotation -= 360;
            
            pointsContainer.Add(newRotation);
            if (_magicPoints.Count > AverageAccuracy) pointsContainer.RemoveAt(0);

            float averageRotation = pointsContainer.Sum() / pointsContainer.Count;
            
            pointerTransform.rotation = Quaternion.Euler(new Vector3(0,0,averageRotation));;   
        }
    }
}
