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

    public EntranceObject NextEntrance()
    {
        var result = LevelManager.instances.FirstOrDefault
        (
            e => e.Id == Id &&
            e.GetComponent<EntranceObject>().Level.ToLower() == Level.ToLower()
        );
        return result ? result.GetComponent<EntranceObject>() : null;
    }

    private void Awake()
    {
        if (meta["passed"].Any(t => ((string)t).ToLower() == Level.ToLower()))
        {
            Passed = true;
            Open = true;

            coreEffect.Emission(true);
        }

        CameraManager.Position = transform.position;
    }

    private void Start()
    {
        if ((string)meta["selected"] == Level)
            CameraManager.Position = transform.position;

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

    public void Connect(Vector2 destination)
    {
        var line = Instantiate(connectionLine, connectionLine.transform.parent);

        var position = (Vector2)transform.position;
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
            enterButton.interactable = File.Exists($"{level.Info.Directory}/{Level}.#");
        }
    }

    private int threshold;
    public int Threshold
    {
        get
        {
            return threshold;
        }
        set
        {
            threshold = value;
            gameObject.SetActive(Threshold <= 0);
        }
    }

    public bool Open { get; private set; }

    public string Next { get; private set; }

    public override void Serialize(JToken token)
    {
        token["open"] = Open;
        token["level"] = Level;
        token["threshold"] = Threshold;
        token["description"] = descriptionText.text;
        token["next"] = Next;
    }

    public override void Deserialize(JToken token)
    {
        Open = (bool)token["open"];
        Level = (string)token["level"];
        Threshold = (int)token["threshold"];
        descriptionText.text = (string)token["description"];
        Next = (string)token["next"];
    }

    #endregion
}