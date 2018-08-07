using UnityEngine;

public abstract class BasePainter<T> : MonoBehaviour where T : Component
{
    [SerializeField]
    private ColorVariable variable;
    public ColorVariable Variable
    {
        get
        {
            return variable;
        }
        set
        {
            variable = value;
            Refresh();
        }
    }

    protected T component;

    private void Awake()
    {
        component = GetComponent<T>();
        Refresh();
    }

    public abstract void Refresh();
}