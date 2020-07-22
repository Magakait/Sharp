using UnityEngine;
using Sharp.Core;
using Newtonsoft.Json.Linq;
using DG.Tweening;

public class SeekerObject : MonoBehaviour, ISerializable
{
    private void Start() =>
        animation = gameObject.AddComponent<TweenArrayComponent>().Init
        (
            DOTween.Sequence().Insert
            (
                transformBody1
                    .DORotate(Constants.Eulers[1], movable.Transition)
                    .SetEase(Ease.Linear),
                transformBody2
                    .DORotate(-Constants.Eulers[1], movable.Transition)
                    .SetEase(Ease.Linear)
            )
                .SetLoops(-1)
                .Play()
        );

    private void Update()
    {
        if (string.IsNullOrEmpty(sequence))
            return;

        Check();
        Move();
    }

    #region gameplay

    [Header("Gameplay")]
    public MovableComponent movable;

    [Space(10)]
    public string sequence;

    private int index;

    private static readonly bool[] checks = new bool[4];
    private void Check()
    {
        for (var i = 0; i < checks.Length; i++)
            checks[i] = movable.CanMove(i);
    }

    private void Move()
    {
        var index = this.index;
        do
        {
            var direction = sequence[index] - '0';
            if (checks[direction])
            {
                movable.Move(direction);
                this.index = index;

                break;
            }
            else
                index = (index + 1) % sequence.Length;
        } while (index != this.index);
    }

    #endregion

    #region animation

    [Header("Animation")]
    [SerializeField]
    private Transform transformBody1;
    [SerializeField]
    private Transform transformBody2;

    private new TweenArrayComponent animation;

    #endregion

    #region serialization

    public void Serialize(JToken token)
    {
        token["transition"] = movable.Transition;
        token["sequence"] = sequence;
    }

    public void Deserialize(JToken token)
    {
        movable.Transition = (float)token["transition"];
        sequence = (string)token["sequence"];
    }

    #endregion
}
