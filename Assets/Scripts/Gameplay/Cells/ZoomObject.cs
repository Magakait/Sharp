using UnityEngine;
using Sharp.Camera;
using Newtonsoft.Json.Linq;

namespace Sharp.Gameplay
{
    [RequireComponent(typeof(Animator))]
    public class ZoomObject : MonoBehaviour, ISerializable
    {
        private Animator animator;

        private void Awake() =>
            animator = GetComponent<Animator>();

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.GetComponent<PlayerObject>())
                return;

            CameraManager.Zoom(45 - 15 * Zoom);
            animator.SetTrigger("Rotate");
        }

        [SerializeField]
        private int zoom;
        public int Zoom
        {
            get => zoom;
            private set
            {
                zoom = value;
                animator.SetInteger("Zoom", Zoom);
            }
        }

        public void Serialize(JToken token) =>
            token["zoom"] = Zoom;

        public void Deserialize(JToken token) =>
            Zoom = (int)token["zoom"];
    }
}
