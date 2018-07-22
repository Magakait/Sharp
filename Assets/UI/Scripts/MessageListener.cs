using UnityEngine.Events;
using UnityEngine;

class MessageListener : MonoBehaviour
{
    public UnityEvent onAwake;
    public UnityEvent onStart;
    public UnityEvent onDestroy;

    public void Awake() => onAwake.Invoke();

    public void Start() => onStart.Invoke();

    public void OnDestroy() => onDestroy.Invoke();
}