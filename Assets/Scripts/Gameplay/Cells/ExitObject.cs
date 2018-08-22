using System.Linq;

using UnityEngine;

using Newtonsoft.Json.Linq;

public class ExitObject : SerializableObject
{
    private void Awake() =>
        Passed = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!Passed && collision.GetComponent<PlayerObject>())
        {
            Pass();
            collision.GetComponent<UnitComponent>().Kill();
        }
    }

    #region gameplay

    [Header("Gameplay")]
    [SerializeField]
    private JsonFile meta;
    [SerializeField]
    private JsonFile level;

    public static bool Passed { get; private set; }

    private void Pass()
    {
        Passed = true;

        var passed = (JArray)meta["passed"];
        if (!passed.Select(t => (string)t).Contains(level.ShortName))
        {
            passed.Add(level.ShortName);
            meta.Save();
        }
    }

    #endregion
}