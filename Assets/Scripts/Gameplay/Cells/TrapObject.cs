using UnityEngine;

using DG.Tweening;
using Newtonsoft.Json.Linq;

public class TrapObject : SerializableObject
{
    private void Awake() =>
        animation = gameObject.AddComponent<TweenArrayComponent>().Init
        (
            DOTween.Sequence().Insert
            (
                spikesTransform
                    .DOScale(0, Constants.Time)
            )
        );
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (state.State == 1)
            collision.GetComponent<UnitComponent>().Kill();
    }

    #region gameplay

    [Header("Gameplay")]
    [SerializeField]
    private CellComponent cell;
    [SerializeField]
    private StateComponent state;

    public void Switch()
    {
        bool active = state.State == 1;

        if (active)
            foreach (UnitComponent unit in cell.GetCollisions<UnitComponent>())
                unit.Kill();

        animation[0].Play(active);
    }

    #endregion

    #region animation

    [Header("Animation")]
    [SerializeField]
    private Transform spikesTransform;

    private new TweenArrayComponent animation;

    #endregion

    #region serialization

    public override void Serialize(JToken token) =>
        token["active"] = state.State == 1;

    public override void Deserialize(JToken token) =>
        state.State = (bool)token["active"] ? 1 : 0;

    #endregion
}
