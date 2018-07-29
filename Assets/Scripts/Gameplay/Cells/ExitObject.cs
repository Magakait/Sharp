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
    public ActionMessage message;

    [Space(10)]
    public JsonFile collectionFile;
    public JsonFile levelFile;

    public static bool Passed { get; private set; }

    private void Pass()
    {
        Passed = true;

        JArray levels = (JArray)collectionFile["levels"];
        int current = 1 + levels
            .Select(i => (string)i)
            .ToList()
            .IndexOf(levelFile.FileNameWithoutExtension);

        if (current > (int)collectionFile["current"])
        {
            collectionFile["current"] = current;
            collectionFile.Save();
        }

        Instantiate(message, transform.position, Quaternion.identity)
            .Setup
            (
                "+ + + +",
                "Next",
                () =>
                {
                    if (current < levels.Count)
                    {
                        levelFile.Load($"{levelFile.Directory}/{(string)levels[current]}.#");
                        EngineUtility.Main.OpenScene();
                    }
                    else
                        EngineUtility.Main.OpenScene("Home");
                }
            );
    }

    #endregion
}