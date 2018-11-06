using System.IO;

using UnityEngine;

using Facepunch.Steamworks;

public class SteamManager : MonoBehaviour
{
    private static Client client;

    private void Awake()
    {
        Facepunch.Steamworks.Config.ForUnity(Application.platform.ToString());
        client = new Facepunch.Steamworks.Client((uint)int.Parse(File.ReadAllText("steam_appid.txt")));
    }

    private void OnDestroy() => client.Dispose();

    public static void UnlockAchievement(string id) => client.Achievements.Find(id).Trigger();
}