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

    public bool Valid { get; private set; }
    public bool Passed { get; private set; }

    public static EntranceObject FindByLevel(string level)
    {
        // level = level.ToLower();
        // var result = LevelManager.instances.FirstOrDefault
        // (
        //     e => e.Id == Id &&
        //     e.GetComponent<EntranceObject>().Level.ToLower() == level
        // );
        // return result ? result.GetComponent<EntranceObject>() : null;
        return null;
    }

    private void Start()
    {
        // if (!Valid || !Open)
        // {
        //     gameObject.SetActive(false);
        //     return;
        // }

        // var next = NextEntrance();
        // if (Open && (!next || !next.Open))
        // {
        //     CameraManager.Position = transform.position;
        //     if (!Passed)
        //         haloEffect.Emission(true);
        // }

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

    public void Pass()
    {
        // Passed = true;
        // Open = true;

        // var next = NextEntrance();
        // if (next)
        // {
        //     next.Open = true;
        //     Connect(next.transform.position);
        // }

        coreEffect.Emission(true);
    }

    public void Connect(Vector2 destination)
    {
        var line = Instantiate(connectionLine, connectionLine.transform.parent);
        line.gameObject.SetActive(true);

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
            Valid = File.Exists($"{level.Info.Directory}/{Level}.#");
        }
    }

    public int Threshold { get; private set; }

    public string Connections { get; private set; }

    public override void Serialize(JToken token)
    {
        token["threshold"] = Threshold;
        token["level"] = Level;
        token["description"] = descriptionText.text;
        token["connections"] = Connections;
    }

    public override void Deserialize(JToken token)
    {
        Threshold = (int)token["threshold"];
        Level = (string)token["level"];
        descriptionText.text = (string)token["description"];
        Connections = (string)token["connections"];
    }

    #endregion
}