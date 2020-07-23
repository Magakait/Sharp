using UnityEngine;
using Sharp.Core;
using Newtonsoft.Json.Linq;
using DG.Tweening;

namespace Sharp.Gameplay
{
    public class FogObject : MonoBehaviour, ISerializable
    {
        private void Awake() =>
            animation = gameObject.AddComponent<TweenContainer>().Init
            (
                DOTween.Sequence().Insert
                (
                    maskSprite
                        .DOFade(0, Constants.Time)
                )
            );

        private void Start() =>
            collider.radius = 1;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponent<PlayerObject>())
                Reveal();
        }

        #region gameplay

        [Header("Gameplay")]
        public new CircleCollider2D collider;

        [SerializeField]
        private bool spread;
        public bool Spread { get => spread; set => spread = value; }

        private void Reveal()
        {
            collider.enabled = false;

            Destroy(gameObject, 0.1f);
            if (Spread)
                Invoke("Delay", .075f);

            animation[0].Play(false);
        }

        private void Delay()
        {
            while (true)
            {
                Collider2D neighbor = Physics2D.OverlapPoint(transform.position, Constants.FogMask);
                if (neighbor)
                    neighbor.GetComponent<FogObject>().Reveal();
                else
                    break;
            }
        }

        #endregion

        #region animation

        [Header("Animation")]
        [SerializeField]
        private SpriteRenderer maskSprite;

        private new TweenContainer animation;

        #endregion

        #region serialization

        public void Serialize(JToken token) =>
            token["spread"] = Spread;

        public void Deserialize(JToken token) =>
            Spread = (bool)token["spread"];

        #endregion
    }
}
