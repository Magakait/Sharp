using UnityEngine;

public abstract class BaseCosmetic<T> : MonoBehaviour
{
    [SerializeField]
    private T variable;
    public T Variable
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

    public static void RefreshAll()
    {
        foreach (BaseCosmetic<T> cosmetic in FindObjectsOfType<BaseCosmetic<T>>())
            cosmetic.Refresh();
    }
}