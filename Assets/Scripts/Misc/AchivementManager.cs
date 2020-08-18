using System.IO;
using UnityEngine;
using Facepunch.Steamworks;

namespace Sharp.Managers
{
    public class AchivementManager : MonoBehaviour
    {
        private static Client client;

        private void Awake()
        {
            if (client != null)
                return;
            try
            {
                Facepunch.Steamworks.Config.ForcePlatform(OperatingSystem.Windows, Architecture.x64);
                client = new Facepunch.Steamworks.Client(uint.Parse(File.ReadAllText("steam_appid.txt")));
            }
            catch { }
        }

        private void Update() =>
            client.Update();

        private void OnDestroy() =>
            client.Dispose();

        private void Unlock(string id)
        {
            try
            {
                client.Achievements.Find(id).Trigger();
            }
            catch { }
        }

        public void UnlockSet()
        {
            if (SetManager.Category == "Factory" && (bool)SetManager.Meta["completed"])
                Unlock(SetManager.Name);
        }

        public void UnlockRoll() =>
            Unlock("alkaitagi");
    }
}
