using UnityEngine;

public class ToggleInverse : MonoBehaviour
{
    [SerializeField]
    private bool value;
    [SerializeField]
    private BoolEvent onInvoke;

    public void Invoke(bool value)
    {
        this.value = value;
        Invoke();
    }

    public void Invoke() => onInvoke.Invoke(value);

    public void Inverse(bool value) => Invoke(!value);

    public void Inverse() => Invoke(!value);
}