using UnityEngine;

using DG.Tweening;
using Newtonsoft.Json.Linq;

public class SeekerObject : SerializableObject
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
        if (!movable.Moving() && !string.IsNullOrEmpty(sequence))
        {
            Check();
            Move();
        }
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
        for (int i = 0; i < checks.Length; i++)
            checks[i] = movable.CanMove(i);
    }

    private void Move()
    {
        int index = this.index;
        do
        {
            int direction = sequence[index] - '0';
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

    public override void Serialize(JToken token)
    {
        token["transition"] = movable.Transition;
        token["sequence"] = sequence;
    }

    public override void Deserialize(JToken token)
    {
        movable.Transition = (float)token["transition"];
        sequence = (string)token["sequence"];
    }

    #endregion
}