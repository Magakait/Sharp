﻿using UnityEngine;
using UnityEngine.UI;

using Newtonsoft.Json.Linq;

public class LogObject : SerializableObject
{
    private void Start() =>
        burst.transform.rotation = Quaternion.FromToRotation(Vector3.up, emptyRect.localPosition);

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!Persistent && collision.GetComponent<PlayerObject>())
            Switch(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!Persistent && collision.GetComponent<PlayerObject>())
            Switch(false);
    }

    #region gameplay

    [Header("Gameplay")]
    public Text text;
    public RectTransform panelRect;
    public RectTransform emptyRect;
    public CanvasToggle canvasToggle;

    [Space(10)]
    [SerializeField]
    private bool persistent;
    public bool Persistent
    {
        get
        {
            return persistent;
        }
        set
        {
            persistent = value;
            Switch(Persistent);
        }
    }

    private void Switch(bool visible)
    {
        canvasToggle.Visible = visible;
        burst.Emission(visible);
    }

    #endregion

    #region animation

    [SerializeField]
    private ParticleSystem burst;

    #endregion

    #region serialization

    public override void Serialize(JToken token)
    {
        token["text"] = text.text;
        token["size"] = ((Vector3)panelRect.sizeDelta).ToJToken();
        token["offset"] = emptyRect.localPosition.ToJToken();
        token["persistent"] = Persistent;
    }

    public override void Deserialize(JToken token)
    {
        text.text = (string)token["text"];
        panelRect.sizeDelta = token["size"].ToVector();
        emptyRect.localPosition = token["offset"].ToVector();
        Persistent = (bool)token["persistent"];
    }

    #endregion
}