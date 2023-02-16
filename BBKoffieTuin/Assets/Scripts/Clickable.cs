using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Clickable : MonoBehaviour , IPointerDownHandler , IPointerUpHandler
{
    public bool isPressing;

    public UnityEvent onHold = new UnityEvent();
    public UnityEvent onButtonDown = new UnityEvent();
    public UnityEvent onButtonUp = new UnityEvent();

    private void Awake()
    {
        onButtonDown.AddListener(()=>{Debug.Log("Down");});
        onButtonUp.AddListener(()=>{Debug.Log("Up");});
    }

    private void Update()
    {
        if (isPressing)
        {
            onHold?.Invoke();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPressing = true;
        onButtonDown.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPressing = false;
        onButtonUp.Invoke();
    }
}