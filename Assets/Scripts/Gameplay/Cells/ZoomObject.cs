using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;
using Newtonsoft.Json.Linq;

public class ZoomObject : SerializableObject
{
    private Tweener tweener;

    private void Awake() =>
        tweener = CameraMain.Camera
            .DOFieldOfView(Zoom, Constants.Time);


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerObject>())
            tweener
                .ChangeValues(CameraMain.Camera.fieldOfView, Zoom)
                .Restart();

    }

    private void OnDestroy() => tweener.Kill();

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
    private Transform body;
    [SerializeField]
    private Transform frame;

    #endregion

    #region serialization

    public override void Serialize(JToken token) => token["zoom"] = Zoom;

    public override void Deserialize(JToken token) => Zoom = (int)token["zoom"];

    #endregion
}