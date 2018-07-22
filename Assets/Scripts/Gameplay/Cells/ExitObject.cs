using System.Linq;

using UnityEngine;

using Newtonsoft.Json.Linq;

public class ExitObject : SerializableObject
{
    private void Awake() =>
        Activated = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!Activated && collision.GetComponent<PlayerObject>())
        {
            Activate();
            collision.GetComponent<UnitComponent>().Kill();
        }
    }

    #region gameplay

    [Header("Gameplay")]
    public ActionMessage message;

    [Space(10)]
    public JsonFile collectionFile;
    public JsonFile levelFile;

    public static bool Activated { get; private set; }

    private void Activate()
    {
        Activated = true;

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