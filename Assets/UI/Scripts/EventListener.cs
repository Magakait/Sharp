using UnityEngine;

namespace AlKaitagi.SharpUI
{
    public class EventListener : MonoBehaviour
    {
        [SerializeField]
        private VoidEvent onAwake = null;
        [SerializeField]
        private VoidEvent onStart = null;
        [SerializeField]
        private VoidEvent onDestroy = null;

        private void Awake() => onAwake.Invoke();
        private void Start() => onStart.Invoke();
        private void OnDestroy() => onDestroy.Invoke();
    }
}
