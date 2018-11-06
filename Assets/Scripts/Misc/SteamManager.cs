using System.IO;

using UnityEngine;

using Facepunch.Steamworks;

public class SteamManager : MonoBehaviour
{
    private static Client client;

    private void Awake()
    {
        if (client != null)
            return;

        Facepunch.Steamworks.Config.ForcePlatform(OperatingSystem.Windows, Architecture.x64);
        client = new Facepunch.Steamworks.Client((uint)int.Parse(File.ReadAllText("steam_appid.txt")));
    }

    private void Update() => client.Update();

    private void OnDestroy() => client.Dispose();

    public static void UnlockAchievement(string id)
    {
        try
        {
            client.Achievements.Find(id).Trigger();
        }
        catch { }
    }
}