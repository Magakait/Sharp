using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class KeyListener : MonoBehaviour
{
    [SerializeField]
    private KeyVariable key;
    [SerializeField]
    private UnityEvent onDown;

    private void Update()
    {
        if (Keyboard.current[key].isPressed && LoadingScreen.Ready && !EngineUtility.IsInput)
            Invoke();
    }

    public void Invoke() => onDown.Invoke();
}
