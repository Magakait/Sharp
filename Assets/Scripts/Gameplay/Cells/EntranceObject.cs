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
    private new CircleCollider2D collider;

    public bool Valid { get; private set; }
    public bool Passed { get; private set; }

    private EntranceObject NextEntrance()
    {
        var result = LevelManager.instances.FirstOrDefault
        (
            e => e.Id == Id &&
            e.GetComponent<EntranceObject>().Level.ToLower() == Next.ToLower()
        );
        return result ? result.GetComponent<EntranceObject>() : null;
    }

    private void Start()
    {
        if (!Valid || !Open)
        {
            gameObject.SetActive(false);
            return;
        }

        var next = NextEntrance();
        if (Open && (!next || !next.Open))
            Focus();

        collider.radius = 1;
    }

    private void OnMouseDown()
    {
        if (enabled && Open && !EngineUtility.IsOverUI)
            CameraManager.Move(transform.position);
    }

    public void Enter()
    {
        level.Load($"{level.Info.Directory}/{Level}.#");
        EngineUtility.Main.LoadScene("Play");
    }

    public void Pass()
    {
        Passed = true;
        Open = true;

        var next = NextEntrance();
        if (next)
        {
            next.Open = true;
            Connect(next.transform.position);
        }

        coreEffect.Emission(true);
    }

    public void Connect(Vector2 destination)
    {
        var position = (Vector2)transform.position;
        var offset = .75f * (destination - position).normalized;

        connectionLine.SetPosition(0, position + offset);
        connectionLine.SetPosition(1, .5f * (position + destination));
        connectionLine.SetPosition(2, destination - offset);
    }

    private void Focus()
    {
        CameraManager.Position = transform.position;
        if (!Passed)
            haloEffect.Emission(true);
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

    public bool Open { get; private set; }

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