using UnityEngine;
using UnityEngine.Events;

public class KeyListener : MonoBehaviour
{
    [SerializeField]
    private KeyVariable key;
    [SerializeField]
    private UnityEvent onDown;

    private void Update()
    {
        if (Input.GetKeyDown(key) && LoadingScreen.Ready && !EngineUtility.IsInput)
            Invoke();
    }

    public void Invoke() => onDown.Invoke();
}