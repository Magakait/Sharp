using UnityEngine;

using Facepunch.Steamworks;
using System.Threading;

public class EditorWorkshop : MonoBehaviour
{
    private Workshop.Editor item;

    private void Awake()
    {
        if (SetManager.Info["id"] != null)
            item = SteamManager.Client.Workshop.EditItem((ulong)SetManager.Info["id"]);
    }

    public void Upload()
    {
        if (item == null)
        {
            item = SteamManager.Client.Workshop.CreateItem
            (
                SteamManager.Client.AppId,
                Workshop.ItemType.Community
            );

            SetManager.Info["id"] = item.Id;
            SetManager.Info.Save();
        }

        item.Title = SetManager.Name;
        item.Description = FixTags((string)SetManager.Info["description"]);
        item.Folder = SetManager.FullName;
        item.Type = Workshop.ItemType.Community;
        item.Visibility = Workshop.Editor.VisibilityType.Public;
        item.Publish();
    }

    private static string FixTags(string text)
    {
        text = text.Replace("<b>", "[b]");
        text = text.Replace("</b>", "[/b]");

        text = text.Replace("<i>", "[i]");
        text = text.Replace("</i>", "[/i]");

        text = text.Replace("</color>", string.Empty);

        return text;
    }
}