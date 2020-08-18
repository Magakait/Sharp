using UnityEngine;
using UnityEngine.InputSystem;
using Sharp.UI;

namespace Sharp.Camera
{
    public class CameraZoom : MonoBehaviour
    {
        [SerializeField] private float speed;
        [Space(10)]
        [SerializeField] private float minFOV;
        [SerializeField] private float maxFOV;

        private void Update()
        {
            if (UIUtility.IsOverUI)
                return;

            var scroll = speed * Mouse.current.scroll.ReadValue().y;
            if (scroll == 0)
                return;

            var zoom = Mathf.Clamp(CameraManager.FieldOfView - scroll, minFOV, maxFOV);
            CameraManager.Zoom(zoom, .5f);
        }

        private void Start() =>
            CameraManager.FieldOfView = minFOV;

        private void OnDisable() =>
             Start();
    }
}
