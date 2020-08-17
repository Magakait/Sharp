using UnityEngine;
using Cinemachine;

namespace AlKaitagi.Camera
{
    [AddComponentMenu("")]
    public class CameraMouseTilt : CinemachineExtension
    {
        [SerializeField, Range(0, 1)]
        private float radialClamp = .9f;
        [SerializeField]
        private float scale = 5;

        protected override void PostPipelineStageCallback(
            CinemachineVirtualCameraBase vcam,
            CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
        {
            if (stage != CinemachineCore.Stage.Aim)
                return;

            var mouse = CameraMouseUtility.GetMouseOnScreen(radialClamp);
            var tilt = scale * new Vector2(.5f - mouse.y, mouse.x - .5f);
            state.OrientationCorrection *= Quaternion.Euler(tilt);
        }
    }
}
