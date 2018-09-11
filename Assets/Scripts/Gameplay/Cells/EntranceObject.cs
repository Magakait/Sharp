using System.IO;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;

using Newtonsoft.Json.Linq;

public class EntranceObject : SerializableObject
{
    [Space(10)]
    [SerializeField]
    private JsonFile level;
    [SerializeField]
    private JsonFile meta;
    [SerializeField]
    private new CircleCollider2D collider;

    public bool Passed { get; private set; }

    private void Awake()
    {
        if (meta["passed"].Any(t => ((string)t).ToLower() == Level.ToLower()))
        {
            Passed = true;
            coreEffect.Emission(true);
        }

        CameraManager.Position = transform.position;
    }

    private void Start()
    {
        if ((string)meta["selected"] == Level)
            CameraManager.Position = transform.position;

        Threshold = Threshold;

        enterButton.interactable = File.Exists($"{level.Info.Directory}/{Level}.#");
        collider.radius = 1;
    }

    private void OnMouseDown()
    {
        if (enabled && !EngineUtility.IsOverUI)
        {
            meta["selected"] = Level;
            meta.Save();

            CameraManager.Move(transform.position);
        }
    }

    public void Enter()
    {
        if (enabled)
        {
            level.Load($"{level.Info.Directory}/{Level}.#");
            EngineUtility.Main.LoadScene("Play");
        }
    }

    public void Connect(EntranceObject target)
    {
        target.Threshold--;

        var line = Instantiate(connectionLine, connectionLine.transform.parent);

        var position = (Vector2)transform.position;
        var destination = (Vector2)target.transform.position;
        var offset = .75f * (destination - position).normalized;

        line.SetPosition(0, position + offset);
        line.SetPosition(1, .5f * (position + destination));
        line.SetPosition(2, destination - offset);
    }

    #region animation

    [Header("Animation")]
    [SerializeField]
    private ParticleSystem coreEffect;
    [SerializeField]
    private ParticleSystem haloEffect;
    [SerializeField]
    private LineRenderer connectionLine;

    [Space(10)]
    [SerializeField]
    private Text titleText;
    [SerializeField]
    private Text descriptionText;
    [SerializeField]
    private Button enterButton;

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

    private int threshold;
    public int Threshold
    {
        get
        {
            return threshold;
        }
        private set
        {
            threshold = value;
            gameObject.SetActive(Threshold <= 0);
            haloEffect.Emission(gameObject.activeSelf && !Passed);
        }
    }

    public string Connections { get; private set; }

    public override void Serialize(JToken token)
    {
        token["level"] = Level;
        token["description"] = descriptionText.text;
        token["threshold"] = Threshold;
        token["connections"] = Connections;
    }

    public override void Deserialize(JToken token)
    {
        Level = (string)token["level"];
        descriptionText.text = (string)token["description"];
        Threshold = (int)token["threshold"];
        Connections = (string)token["connections"];
    }

    #endregion
}