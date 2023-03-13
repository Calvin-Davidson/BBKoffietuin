using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MultiTouchScrollRect : ScrollRect
{
    #region Fields
    private int _minimumTouchCount = 1;
    private int _maximumTouchCount = 2;
    private int _initialTouchCount = 0;
    #endregion

    #region Properties
    public Vector2 MultiTouchPosition
    {
        get
        {
            Vector2 position = Vector2.zero;
            for (int i = 0; i < Input.touchCount && i < _maximumTouchCount; i++)
            {
                position += Input.touches[i].position;
            }
            position /= ((Input.touchCount <= _maximumTouchCount) ? Input.touchCount : _maximumTouchCount);

            return position;
        }
    }
    #endregion

    #region Methods
    private void Update()
    {
        if (SystemInfo.deviceType == DeviceType.Handheld)
        {
            if (Input.touchCount > 0)
            {
                if (_initialTouchCount == 0)
                {
                    _initialTouchCount = Input.touchCount;
                }
            }
            else
            {
                _initialTouchCount = 0;
            }
        }
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        if (SystemInfo.deviceType == DeviceType.Handheld)
        {
            if (Input.touchCount < _minimumTouchCount || Input.touchCount != _initialTouchCount) return;
            eventData.position = MultiTouchPosition;
            base.OnBeginDrag(eventData);
        }
        else if (SystemInfo.deviceType == DeviceType.Desktop)
        {
            base.OnBeginDrag(eventData);
        }
    }
    public override void OnDrag(PointerEventData eventData)
    {
        if (SystemInfo.deviceType == DeviceType.Handheld)
        {
            if (Input.touchCount < _minimumTouchCount || Input.touchCount != _initialTouchCount) return;
            eventData.position = MultiTouchPosition;
            base.OnDrag(eventData);
        }
        else if (SystemInfo.deviceType == DeviceType.Desktop)
        {
            base.OnDrag(eventData);
        }          
    }
    public override void OnEndDrag(PointerEventData eventData)
    {
        if (SystemInfo.deviceType == DeviceType.Handheld)
        {
            if (Input.touchCount < _minimumTouchCount) return;
            eventData.position = MultiTouchPosition;
            base.OnEndDrag(eventData);
        }
        else if (SystemInfo.deviceType == DeviceType.Desktop)
        {
            base.OnEndDrag(eventData);
        }       
    }
    #endregion
}