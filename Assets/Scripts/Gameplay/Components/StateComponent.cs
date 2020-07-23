using UnityEngine;
using Sharp.Core.Events;

public class StateComponent : MonoBehaviour
{
    [SerializeField]
    private int state;
    public int State
    {
        get => state;
        set
        {
            state = (int)Mathf.Repeat(value, Capacity);
            onSwitch.Invoke(State);
        }
    }
    [SerializeField]
    private int capacity;
    public int Capacity
    {
        get => capacity;
        set
        {
            capacity = value;
            State = State;
        }
    }
    [Space(10)]
    public IntEvent onSwitch;
}
