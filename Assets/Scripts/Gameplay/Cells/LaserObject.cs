using System.Collections.Generic;
using UnityEngine;
using Sharp.Core;
using Newtonsoft.Json.Linq;
using DG.Tweening;

namespace Sharp.Gameplay
{
    public class LaserObject : MonoBehaviour, ISerializable
    {
        private void Awake() =>
            animation = gameObject.AddComponent<TweenContainer>().Init
            (
                DOTween.Sequence().Insert
                (
                    leftTransform
                        .DOLocalMoveX(-.15f, Constants.Time),
                    rightTransform
                        .DOLocalMoveX(.15f, Constants.Time)
                )
                    .SetLoops(2, LoopType.Yoyo),
                DOTween.Sequence().Insert
                (
                    persistencyTransform
                        .DOScale(0, Constants.Time)
                )
            );

        private void OnTriggerEnter2D(Collider2D other)
        {
            active = true;
            enabled = true;

            var main = distanceScaler.ParticleSystem.main;
            main.startColor = main.startColor.color.Fade(1);

            distanceScaler.ParticleSystem.Refresh();
            animation[0].Restart();
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (Persistent)
                active = true;
        }

        private void FixedUpdate()
        {
            if (active)
            {
                Burst();
                active = false;
            }
            else
            {
                var main = distanceScaler.ParticleSystem.main;
                main.startColor = main.startColor.color.Fade(0.3f);

                enabled = false;
            }
        }

        #region gameplay

        [Space(10)]
        [SerializeField]
        private StateComponent state;

        private readonly static List<UnitComponent> units = new List<UnitComponent>();

        private bool active;

        private void Burst()
        {
            var from = (Vector2)transform.position + .5f * Constants.Directions[Direction];
            var to = from + Distance * Constants.Directions[Direction];

            PhysicsUtility.OverlapArea(units, from, to, Constants.UnitMask);
            foreach (var unit in units)
                unit.Kill();
        }

        #endregion

        #region animation

        [Header("Animation")]
        [SerializeField]
        private Transform leftTransform;
        [SerializeField]
        private Transform rightTransform;
        [SerializeField]
        private Transform persistencyTransform;

        [Space(10)]
        [SerializeField]
        private ParticleScalerComponent distanceScaler;

        private new TweenContainer animation;

        #endregion

        #region serialization

        public int Direction
        {
            get => state.State;
            private set => state.State = value;
        }

        [Header("Serialization")]
        [SerializeField]
        private int distance;
        public int Distance
        {
            get => distance;
            private set
            {
                distance = value;
                distanceScaler.Scale(new Vector3(Distance, 0, 0));
                distanceScaler.transform.localPosition = .5f * new Vector3(0, Distance, 0);
            }
        }

        [SerializeField]
        private bool persistent;
        public bool Persistent
        {
            get => persistent;
            private set
            {
                persistent = value;
                animation[1].Play(Persistent);
            }
        }

        public void Serialize(JToken token)
        {
            token["direction"] = Direction;
            token["distance"] = Distance;
            token["persistent"] = Persistent;
        }

        public void Deserialize(JToken token)
        {
            Direction = (int)token["direction"];
            Distance = (int)token["distance"];
            Persistent = (bool)token["persistent"];
        }

        #endregion
    }
}
