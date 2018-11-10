using UnityEngine;

public abstract class BaseAction : ScriptableObject
{
    [SerializeField]
    private Sprite shape;
    public Sprite Shape => shape;

    public abstract void Do(PlayerObject player);
}