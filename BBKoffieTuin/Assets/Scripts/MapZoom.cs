using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Map zoom works by scaling the content parent of the scrollview to a certain size.
/// </summary>
public class MapZoom : MonoBehaviour
{
    [SerializeField] private float currentZoom = 1f;
    [SerializeField] float zoomModifierSpeed = 0.1f;
    [SerializeField] float zoomOutMax = 1f;
    [SerializeField] float zoomInMax = .1f;
    
    private Camera _camera;

    private float _touchesPrevPosDifference;
    private float _touchesCurPosDifference;
    private float _zoomModifier;

    private Vector2 _firstTouchPrevPos;
    private Vector2 _secondTouchPrevPos;

    private void Awake()
    {
        _camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (_camera == null) return;
        if (Input.touchCount != 2) return;

        Touch firstTouch = Input.GetTouch(0);
        Touch secondTouch = Input.GetTouch(1);

        //check if the touches are both on the UI element
        // Ray firstRay = _camera.ScreenPointToRay(firstTouch.position);
        // Ray secondRay = _camera.ScreenPointToRay(secondTouch.position);

        // Check if the touches are on the current element
        // if (!Physics.Raycast(firstRay, out RaycastHit firstHit) || firstHit.transform != transform) return;
        // if (!Physics.Raycast(secondRay, out RaycastHit secondHit) || secondHit.transform != transform) return;
        
        //the logic of zooming in and out 
        _firstTouchPrevPos = firstTouch.position - firstTouch.deltaPosition;
        _secondTouchPrevPos = secondTouch.position - secondTouch.deltaPosition;
        
        _touchesPrevPosDifference = (_firstTouchPrevPos - _secondTouchPrevPos).magnitude;
        _touchesCurPosDifference = (firstTouch.position - secondTouch.position).magnitude;
        
        _zoomModifier = (firstTouch.deltaPosition - secondTouch.deltaPosition).magnitude * zoomModifierSpeed;

        var newScale = transform.localScale;
        if (_touchesPrevPosDifference > _touchesCurPosDifference)
        {
            newScale -= new Vector3(_zoomModifier, _zoomModifier, _zoomModifier);
        }
        if (_touchesPrevPosDifference < _touchesCurPosDifference)
        {
            newScale += new Vector3(_zoomModifier, _zoomModifier, _zoomModifier);
        }
        
        if(newScale.x < zoomInMax) newScale.Set(zoomInMax, zoomInMax, zoomInMax);
        if(newScale.x > zoomOutMax) newScale.Set(zoomOutMax, zoomOutMax, zoomOutMax); 
        currentZoom = Mathf.Clamp(newScale.x, zoomOutMax, zoomInMax);
        transform.localScale = newScale;
    }
}