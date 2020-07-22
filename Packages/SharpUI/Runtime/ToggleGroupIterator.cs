using UnityEngine;
using UnityEngine.UI;

public class ToggleGroupIterator : MonoBehaviour
{
    [SerializeField]
    private int current;
    public int Current
    {
        get => current;
        private set
        {
            current = value % toggles.Length;
            toggles[Current].isOn = true;
        }
    }

    public Toggle[] toggles;

    private void Start() =>
        Current = Current;

    public void Next() =>
        Current++;
}
