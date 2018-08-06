using UnityEngine;

using DG.Tweening;
using Newtonsoft.Json.Linq;

public class GateObject : SerializableObject
{
    [Space(10)]
    [SerializeField]
    private CellComponent cell;
    [SerializeField]
    private StateComponent state;

    private void Awake() =>
        animation = gameObject.AddComponent<TweenArrayComponent>().Init
        (
            DOTween.Sequence().Insert
            (
                cellTransform
                    .DOScale(0, Constants.Time)
            )
        );

    public void Switch()
    {
        cell.Hollowed = !Open;
        animation[0].Play(Open);
    }

    #region animation

    [Header("Animation")]
    [SerializeField]
    private Transform cellTransform;

    private new TweenArrayComponent animation;

    #endregion

    #region serialization

    public bool Open
    {
        get
        {
            return state.State == 1;
        }
        private set
        {
            state.State = value ? 1 : 0;
        }
    }

    public override void Serialize(JToken token) =>
        token["open"] = Open;

    public override void Deserialize(JToken token) =>
        Open = (bool)token["open"];

    #endregion
}