using UnityEngine;
using Sharp.Core;
using DG.Tweening;
using Sharp.Camera;
using Newtonsoft.Json.Linq;

namespace Sharp.Gameplay
{
    public class ZoomObject : MonoBehaviour, ISerializable
    {
        private void Awake()
        {
            animation = gameObject.AddComponent<TweenContainer>().Init
            (
                DOTween.Sequence().Insert
                (
                    frameIn.DORotate(Constants.Eulers[1], Constants.Time),
                    frameOut.DORotate(-Constants.Eulers[1], Constants.Time)
                ),
                DOTween.Sequence().Insert
                (
                    frameIn.DOScale(0, Constants.Time)
                ),
                DOTween.Sequence().Insert
                (
                    frameOut.DOScale(0, Constants.Time)
                )
            );
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponent<PlayerObject>())
            {
                CameraManager.Zoom(45 - 15 * Zoom);
                animation[0].Restart();
            }
        }

        #region gameplay

        [Space(10)]
        [SerializeField]
        private int zoom;
        public int Zoom
        {
            get => zoom;
            private set
            {
                zoom = value;
                animation[1].Play(Zoom >= 0);
                animation[2].Play(Zoom <= 0);
            }
        }

        #endregion

        #region animation

        [Space(10)]
        [SerializeField]
        private Transform frameIn;
        [SerializeField]
        private Transform frameOut;

        private new TweenContainer animation;

        #endregion

        #region serialization

        public void Serialize(JToken token) =>
            token["zoom"] = Zoom;

        public void Deserialize(JToken token) =>
            Zoom = (int)token["zoom"];

        #endregion
    }
}
