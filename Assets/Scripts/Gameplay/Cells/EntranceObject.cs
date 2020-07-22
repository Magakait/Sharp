using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Sharp.UI;
using DG.Tweening;
using Newtonsoft.Json.Linq;

public class EntranceObject : SerializableObject
{
    [Space(10)]
    [SerializeField]
    private new CircleCollider2D collider;

    public bool Passed { get; private set; }

    private void Awake() =>
        animation = gameObject.AddComponent<TweenArrayComponent>().Init
        (
            DOTween.Sequence().Insert(frame.DOFade(0, .2f).From())
        );

    private void Start()
    {
        if ((string)SetManager.Meta["selected"] == Level)
            CameraManager.Position = transform.position;

        if (Connected == 0 && Threshold > 0)
            gameObject.SetActive(false);
        else if (Connected > 0 && Connected < Threshold)
            canvas.gameObject.SetActive(false);
        else if (Passed)
            coreEffect.Emission(true);

        enterButton.interactable = SetManager.Levels.Contains(Level);
        collider.radius = 1;
    }

    private void OnMouseEnter()
    {
        if (enabled)
            animation[0].Play(false);
    }

    private void OnMouseExit()
    {
        if (enabled)
            animation[0].Play(true);
    }

    private void OnMouseDown()
    {
        if (enabled && !UIUtility.IsOverUI)
        {
            SetManager.Meta["selected"] = Level;
            SetManager.Meta.Save();

            CameraManager.Move(transform.position);
        }
    }

    public void Enter()
    {
        LevelManager.Load(Level);
        UIUtility.Main.LoadScene("Play");
    }

    public void Connect(EntranceObject target)
    {
        target.Connected++;

        var line = Instantiate(connectionLine, connectionLine.transform.parent);

        var position = (Vector2)transform.position;
        var destination = (Vector2)target.transform.position;
        var offset = .65f * (destination - position).normalized;

        line.SetPosition(0, position + offset);
        line.SetPosition(1, .5f * (position + destination));
        line.SetPosition(2, destination - offset);
    }

    #region animation

    [Header("Animation")]
    [SerializeField]
    private SpriteRenderer frame;
    [SerializeField]
    private ParticleSystem coreEffect;
    [SerializeField]
    private LineRenderer connectionLine;

    [Space(10)]
    [SerializeField]
    private Canvas canvas;
    [SerializeField]
    private Text titleText;
    [SerializeField]
    private Button enterButton;

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

    public int Threshold { get; private set; }

    public int Connected { get; private set; }

    public string Connections { get; private set; }

    public override void Serialize(JToken token)
    {
        token["level"] = Level;
        token["threshold"] = Threshold;
        token["connections"] = Connections;
    }

    public override void Deserialize(JToken token)
    {
        Level = (string)token["level"];
        Passed = SetManager.Meta["passed"].Any(t => (string)t == Level);

        Threshold = (int)token["threshold"];
        Connections = (string)token["connections"];
    }

    #endregion
}
