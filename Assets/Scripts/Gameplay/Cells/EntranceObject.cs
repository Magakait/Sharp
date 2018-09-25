using System.IO;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;

using Newtonsoft.Json.Linq;

public class EntranceObject : SerializableObject
{
    [Space(10)]
    [SerializeField]
    private new CircleCollider2D collider;

    public bool Passed { get; private set; }

    private void Start()
    {
        if ((string)CollectionManager.Meta["selected"] == Level)
            CameraManager.Position = transform.position;

        if (Threshold > 0)
            gameObject.SetActive(false);
        else if (!Passed)
            haloEffect.Emission(true);

        enterButton.interactable = CollectionManager.Levels.Contains(Level);
        collider.radius = 1;
    }

    private void OnMouseDown()
    {
        if (enabled && !EngineUtility.IsOverUI)
        {
            CollectionManager.Meta["selected"] = Level;
            CollectionManager.Meta.Save();

            CameraManager.Move(transform.position);
        }
    }

    public void Enter()
    {
        LevelManager.Load(Level);
        EngineUtility.Main.LoadScene("Play");      
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

    public int Threshold { get; private set; }

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
        if (CollectionManager.Meta["passed"].Any(t => (string)t == Level))
        {
            Passed = true;
            coreEffect.Emission(true);
        }

        descriptionText.text = (string)token["description"];
        Threshold = (int)token["threshold"];
        Connections = (string)token["connections"];
    }

    #endregion
}