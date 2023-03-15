using System;
using Route;
using TMPro;
using UnityEngine;

namespace Renderers
{
    public class CodeRenderer : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private TextMeshProUGUI codeText;
        [SerializeField] private int codeSlot = 0;

        private RouteCodeHandler _routeCodeHandler;
        private static readonly int EnableColor = Animator.StringToHash("EnableColor");

        private void Awake()
        {
            _routeCodeHandler = FindObjectOfType<RouteCodeHandler>();
        }

        private void Start()
        {
            _routeCodeHandler.onCodeChange.AddListener(Render);
        }

        public void Render(char code, int slot)
        {
            if (slot != codeSlot) return;

            codeText.text = code.ToString();
            animator.SetTrigger(EnableColor);
        }
    }
}
