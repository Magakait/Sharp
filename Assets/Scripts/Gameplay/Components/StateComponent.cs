using UnityEngine;

public class StateComponent : MonoBehaviour
{
    [SerializeField]
    private int state;
    public int State
    {
        get
        {
            return state;
        }
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
        get
        {
            return capacity;
        }
        set
        {
            capacity = value;
            State = State;
        }
    }

    [Space(10)]
    public IntEvent onSwitch;
}