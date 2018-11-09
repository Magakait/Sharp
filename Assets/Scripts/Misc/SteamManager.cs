using System.IO;

using UnityEngine;

using Facepunch.Steamworks;

public class SteamManager : MonoBehaviour
{
    public static Client Client { get; private set; }

    private void Awake()
    {
        if (Client != null)
            return;

        Facepunch.Steamworks.Config.ForcePlatform(OperatingSystem.Windows, Architecture.x64);
        Client = new Facepunch.Steamworks.Client(uint.Parse(File.ReadAllText("steam_appid.txt")));
    }

    private void Update() => Client.Update();

    private void OnDestroy() => Client.Dispose();

    public static void UnlockAchievement(string id)
    {
        try
        {
            Client.Achievements.Find(id).Trigger();
        }
        catch { }
    }
}