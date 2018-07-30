using System.IO;
using System.Linq;
using System.Collections.Generic;

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
    public bool Passed { get; private set; }

    public static readonly List<EntranceObject> instances = new List<EntranceObject>();

    private void Awake()
    {
        animation = gameObject.AddComponent<TweenArrayComponent>().Init
        (
            DOTween.Sequence().Insert
            (
                frameTransform
                    .DOScale(0, Constants.Time)
            )
        );

        instances.Add(this);
    }

    private void OnDestroy() =>
        instances.Remove(this);

    private void Start()
    {
        var next = instances.FirstOrDefault(e => e.Level == Next);
        if (next)
            Connect(next.transform.position);

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
        Passed = true;
        Open = true;

        var next = instances.FirstOrDefault(e => e.Level == Next);
        if (next)
            next.Open = true;

        coreEffect.Emission(Valid);
    }

    public void Connect(Vector2 destination)
    {
        var position = (Vector2)transform.position;
        var offset = .75f * (destination - position).normalized;

        connectionLine.SetPosition(0, position + offset);
        connectionLine.SetPosition(1, .5f * (position + destination));
        connectionLine.SetPosition(2, destination - offset);
    }

    #region animation

    [Header("Animation")]
    [SerializeField]
    private Transform frameTransform;
    [SerializeField]
    private ParticleSystem coreEffect;
    [SerializeField]
    private LineRenderer connectionLine;

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
        }
    }

    public string Next { get; private set; }

    public override void Serialize(JToken token)
    {
        token["open"] = Open;
        token["level"] = Level;
        token["next"] = Next;
    }

    public override void Deserialize(JToken token)
    {
        Open = (bool)token["open"];
        Level = (string)token["level"];
        Next = (string)token["next"];
    }

    #endregion
}