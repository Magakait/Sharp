using UnityEngine;
using Sharp.Camera;
using Newtonsoft.Json.Linq;

namespace Sharp.Gameplay
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(AudioSource))]
    public class ZoomObject : MonoBehaviour, ISerializable
    {
        private Animator animator;
        private new AudioSource audio;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            audio = GetComponent<AudioSource>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.GetComponent<PlayerObject>())
                return;

            var fieldOfView = 45 - 15 * Zoom;
            if (CameraManager.FieldOfView == fieldOfView)
                return;

            CameraManager.Zoom(fieldOfView);
            animator.SetTrigger("Rotate");
            audio.Play();
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
