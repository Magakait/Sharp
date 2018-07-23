using UnityEngine;

public abstract class BaseCosmetic : MonoBehaviour
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

    private void Awake() => 
        Refresh();

    public abstract void Refresh();
}