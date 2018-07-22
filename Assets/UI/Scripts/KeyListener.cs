using UnityEngine;
using UnityEngine.Events;

public class KeyListener : MonoBehaviour
{
    public KeyVariable key;

    [Space(10)]
    public UnityEvent onDown;

    private void Update()
    {
        if (Input.GetKeyDown(key) && !EngineUtility.IsInput)
            Invoke();
    }

    public void Invoke() =>
        onDown.Invoke();
}