using UnityEngine;
using UnityEngine.UI;

using Newtonsoft.Json.Linq;
using DG.Tweening;

public class EntranceObject : SerializableObject
{
    [Space(10)]
    [SerializeField]
    private JsonFile level;
    [SerializeField]
    private JsonFile meta;

    private void Awake() =>
        animation = gameObject.AddComponent<TweenArrayComponent>().Init
        (
            DOTween.Sequence().Insert
            (
                frameTransform
                    .DOScale(0, Constants.Time)
            ),
            DOTween.Sequence().Insert
            (
                lineNext.transform
                    .DOScale(1, Constants.Time)
            )
        );

    private void OnMouseDown()
    {
        if (enabled && Open)
            CameraManager.Move(transform.position);
    }

    public void Enter()
    {
        level.Load($"{meta.Directory}/{Level}.#");
        EngineUtility.Main.OpenScene("Play");
    }

    public void Pass()
    {
        animation[1].Play();

        var entrance = PhysicsUtility.Overlap<EntranceObject>(Next, Constants.CellMask);
        if (entrance && !entrance.Open)
        {
            entrance.Open = true;

            lineNext.SetPosition(0, (Vector2)transform.position);
            lineNext.SetPosition(1, ((Vector2)transform.position + Next) / 2);
            lineNext.SetPosition(2, Next);
        }
    }

    #region animation

    [Header("Animation")]
    [SerializeField]
    private Transform frameTransform;
    [SerializeField]
    private LineRenderer lineNext;

    [Space(10)]
    [SerializeField]
    private CanvasToggle canvasToggle;
    [SerializeField]
    private Text titleText;

    private new TweenArrayComponent animation;

    #endregion

    #region serialization

    public string Level
    {
        get
        {
            return titleText.text;
        }
        private set
        {
            titleText.text = value;
        }
    }

    private bool open;
    public bool Open
    {
        get
        {
            return open;
        }
        private set
        {
            open = value;
            animation[0].Play(Open);
            canvasToggle.Visible = Open;
            CameraManager.Position = transform.position;
        }
    }

    public Vector2 Next { get; private set; }

    public override void Serialize(JToken token)
    {
        token["level"] = Level;
        token["open"] = Open;
        token["next"] = Next.ToJToken();
    }

    public override void Deserialize(JToken token)
    {
        Level = (string)token["level"];
        Open = (bool)token["open"];
        Next = token["next"].ToVector();
    }

    #endregion
}