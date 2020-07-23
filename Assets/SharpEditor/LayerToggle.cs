using UnityEngine;

namespace Sharp.Editor
{
    public class LayerToggle : MonoBehaviour
    {
        private bool visible;
        public bool Visible
        {
            get => visible;
            set
            {
                visible = value;
                CameraManager.Camera.cullingMask = Visible
                    ? CameraManager.Camera.cullingMask | mask
                    : CameraManager.Camera.cullingMask & ~mask;
            }
        }

        [SerializeField]
        private LayerMask mask;

        private void Awake() =>
            Visible = true;

        private void OnDestroy() =>
            Awake();
    }
}
