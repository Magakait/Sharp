using UnityEngine;
using Cinemachine;

namespace AlKaitagi.Camera
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] private new UnityEngine.Camera camera = null;
        public static UnityEngine.Camera Camera => main.camera;
        [SerializeField] private CinemachineBrain brain = null;
        public static CinemachineBrain Brain => main.brain;
        [SerializeField] private CinemachineVirtualCamera virtualCamera = null;
        public static CinemachineVirtualCamera VirtualCamera => main.virtualCamera;

        [Space(10)]
        [SerializeField] private CinemachineVirtualCamera followCamera = null;
        [SerializeField] private CinemachineVirtualCamera moveCamera = null;

        private static CameraManager main;

        private void Awake()
        {
            if (main)
            {
                Destroy(gameObject);
                return;
            }
            main = this;
            Brain.m_CameraActivatedEvent.AddListener(UpdateCameras);
        }

        private void UpdateCameras(ICinemachineCamera oldc, ICinemachineCamera newc)
        {
            camera = Brain.OutputCamera;
            virtualCamera = (CinemachineVirtualCamera)Brain.ActiveVirtualCamera;
        }

        public static void Follow(Transform target)
        {
            main.followCamera.enabled = true;
            main.moveCamera.enabled = false;
            main.followCamera.Follow = target;
        }

        public static void Move(Vector3 position, Vector3 offset, Vector3 rotation)
        {
            main.followCamera.enabled = false;
            main.moveCamera.enabled = true;

            main.moveCamera.transform.position = position;
            main.moveCamera.GetComponent<CinemachineCameraOffset>().m_Offset = offset;
            main.moveCamera.transform.eulerAngles = rotation;
        }
    }
}
