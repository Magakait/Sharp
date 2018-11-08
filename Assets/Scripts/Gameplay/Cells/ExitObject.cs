using System.Linq;

using UnityEngine;

using Newtonsoft.Json.Linq;

public class ExitObject : SerializableObject
{
    private void Awake() => Passed = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!Passed && collision.GetComponent<PlayerObject>())
        {
            Pass();
            collision.GetComponent<UnitComponent>().Kill();
        }
    }

    public static bool Passed { get; private set; }

    private void Pass()
    {
        Passed = true;

        var passed = (JArray)SetManager.Meta["passed"];
        if (!passed.Any(t => (string)t == LevelManager.Level.ShortName))
        {
            passed.Add(LevelManager.Level.ShortName);
            SetManager.Meta.Save();
        }
    }
}