using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;
using Newtonsoft.Json.Linq;

public class ZoomObject : SerializableObject
{
    private void Awake()
    {
        animation = gameObject.AddComponent<TweenArrayComponent>().Init
        (
            DOTween.Sequence().Insert
            (
                frame
                    .DORotate(Constants.Eulers[1], Constants.Time)
            )
        );
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerObject>())
        {
            var fieldOfView = 45 - 15 * Zoom;
            CameraManager.Zoom(fieldOfView);

            if (fieldOfView > 45)
            {
                animation[0].Complete();
                animation[0].SmoothRewind();
            }
            else
                animation[0].Restart();
        }
    }

    #region gameplay

    [Space(10)]
    [SerializeField]
    private int zoom;
    public int Zoom
    {
        get
        {
            return zoom;
        }
        private set
        {
            zoom = value;
        }
    }

    #endregion

    #region animation

    [Space(10)]
    [SerializeField]
    private Transform frame;

    private new TweenArrayComponent animation;

    #endregion

    #region serialization

    public override void Serialize(JToken token) => token["zoom"] = Zoom;

    public override void Deserialize(JToken token) => Zoom = (int)token["zoom"];

    #endregion
}