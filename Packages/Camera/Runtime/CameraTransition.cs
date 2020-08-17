using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace AlKaitagi.Camera
{
    public class CameraTransition : MonoBehaviour
    {
        [SerializeField] private float delay = 3;
        public float Delay { get => delay; set => delay = value; }

        [SerializeField] private Vector3 offset = new Vector3(0, 0, -7);
        public Vector3 Offset { get => offset; set => offset = value; }

        [SerializeField] private Vector3 rotation = Vector3.zero;
        public Vector3 Rotation { get => rotation; set => rotation = value; }
        
        [SerializeField] private UnityEvent onTransition = null;

        public void Begin()
        {
            StopAllCoroutines();
            StartCoroutine(Transition(Delay,
                                      transform.position,
                                      Offset,
                                      Rotation));
        }

        private IEnumerator Transition(float delay, Vector3 position, Vector3 offset, Vector3 rotation)
        {
            yield return new WaitForSeconds(delay);
            CameraManager.Move(position, offset, rotation);
            onTransition.Invoke();
        }
    }
}
