using UnityEngine;
using Newtonsoft.Json.Linq;
using DG.Tweening;

public class GateObject : MonoBehaviour, ISerializable
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
        get => state.State == 1;
        private set => state.State = value ? 1 : 0;
    }

    public void Serialize(JToken token) =>
        token["open"] = Open;

    public void Deserialize(JToken token) =>
        Open = (bool)token["open"];

    #endregion
}
