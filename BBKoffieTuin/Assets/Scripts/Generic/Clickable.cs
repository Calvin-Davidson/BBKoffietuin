using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Generic
{
    [RequireComponent(typeof(Button))]
    public class Clickable : MonoBehaviour
    {
        private Button _button;
        private void Awake()
        {
            _button = GetComponent<Button>();
            _button ??= gameObject.AddComponent<Button>();
            
            _button.transition = Selectable.Transition.None;
            _button.targetGraphic = null;
            _button.colors = ColorBlock.defaultColorBlock;
            
            _button.onClick.AddListener(() => onClick.Invoke());
            _button.onClick.AddListener(()=>{print(gameObject.name);});
        }

        public UnityEvent onClick = new UnityEvent();
    }
}