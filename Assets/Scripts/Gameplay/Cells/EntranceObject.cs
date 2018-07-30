using System.IO;

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

    public bool Valid { get; private set; }

    private void Awake() =>
        animation = gameObject.AddComponent<TweenArrayComponent>().Init
        (
            DOTween.Sequence().Insert
            (
                frameTransform
                    .DOScale(0, Constants.Time)
            )
        );

    private void Start()
    {
        if (Open && !Valid)
        {
            canvasToggle.Visible = false;
            Pass();
        }
    }

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
        coreEffect.Emission(Valid);

        var entrance = PhysicsUtility.Overlap<EntranceObject>(Next, Constants.CellMask);
        if (entrance && !entrance.Open)
        {
            entrance.Open = true;

            var position = (Vector2)transform.position;
            var offset = .75f * (Next - position).normalized;

            nextLine.SetPosition(0, position + offset);
            nextLine.SetPosition(1, .5f * (position + Next));
            nextLine.SetPosition(2, Next - offset);
        }
    }

    #region animation

    [Header("Animation")]
    [SerializeField]
    private Transform frameTransform;
    [SerializeField]
    private ParticleSystem coreEffect;
    [SerializeField]
    private LineRenderer nextLine;

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
            Valid = File.Exists($"{meta.Directory}/{Level}.#");
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