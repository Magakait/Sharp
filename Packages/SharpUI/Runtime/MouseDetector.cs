using UnityEngine;
using UnityEngine.InputSystem;
using AlKaitagi.SharpCore.Events;

namespace AlKaitagi.SharpUI
{
    [RequireComponent(typeof(RectTransform))]
    public class MouseDetector : MonoBehaviour
    {
        private bool detected;
        public bool Detected
        {
            get => detected;
            private set
            {
                if (Detected == value)
                    return;

                detected = value;
                onMouseDetected.Invoke(Detected);
            }
        }
        [SerializeField]
        private BoolEvent onMouseDetected = null;

        private new RectTransform transform;

        private void Awake() => transform = GetComponent<RectTransform>();

        private void Update() =>
            Detected = RectTransformUtility
                .RectangleContainsScreenPoint(transform, Mouse.current.position.ReadValue());
    }
}
