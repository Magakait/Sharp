using UnityEngine;
using Cinemachine;

namespace AlKaitagi.Camera
{
    [AddComponentMenu("")]
    public class CameraMouseOffset : CinemachineExtension
    {
        [SerializeField, Range(0, 1)]
        private float radialClamp = .9f;
        [SerializeField]
        private float scale = 1;

        protected override void PostPipelineStageCallback(
            CinemachineVirtualCameraBase vcam,
            CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
        {
            if (stage != CinemachineCore.Stage.Aim)
                return;

            var mouse = scale * CameraMouseUtility.GetMouseOnScreen(radialClamp);
            var offset = new Vector3(.5f + mouse.x, .5f + mouse.y);
            state.PositionCorrection += offset;
        }
    }
}
