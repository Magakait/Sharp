using UnityEngine;

namespace Sharp.Camera
{
    [RequireComponent(typeof(UnityEngine.Camera))]
    public class CameraManager : MonoBehaviour
    {
        public static UnityEngine.Camera Camera { get; private set; }

        public static Vector2 Position
        {
            get => new Vector2(Camera.transform.localPosition.x, Camera.transform.localPosition.y);
            set => Camera.transform.localPosition = new Vector3(value.x, value.y, -10);
        }

        public static Vector3 Rotation
        {
            get => Camera.transform.localEulerAngles;
            set => Camera.transform.localEulerAngles = value;
        }

        public static float FieldOfView
        {
            get => Camera.fieldOfView;
            set => Camera.fieldOfView = value;
        }

        private static Vector2? targetPosition = null;
        private static float positionSpeed = 1;

        private static float? targetFOV = null;
        private static float fovSpeed = 1;

        private void Awake()
        {
            if (Camera)
                return;
            Camera = GetComponent<UnityEngine.Camera>();
        }

        public void Stop()
        {
            targetPosition = null;
            targetFOV = null;
        }

        private void LateUpdate()
        {
            if (targetPosition.HasValue)
            {
                var step = positionSpeed * Time.deltaTime;
                Position = Vector3.Lerp(Position, targetPosition.Value, step);
                if (Position == targetPosition.Value)
                    targetPosition = null;
            }
            if (targetFOV.HasValue)
            {
                var step = fovSpeed * Time.deltaTime;
                FieldOfView = Mathf.Lerp(FieldOfView, targetFOV.Value, step);
                if (FieldOfView == targetFOV.Value)
                    targetFOV = null;
            }
        }

        public static void Move(Vector2 position, float transition = 1)
        {
            targetPosition = position;
            positionSpeed = 1 / (.2f * transition);
        }

        public static void Zoom(float fieldOfView, float transition = 1)
        {
            targetFOV = fieldOfView;
            fovSpeed = 1 / (.2f * transition);
        }
    }
}
