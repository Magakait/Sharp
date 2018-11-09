using System.IO;

using UnityEngine;
using UnityEngine.UI;

using Facepunch.Steamworks;

public class EditorWorkshop : MonoBehaviour
{
    [SerializeField]
    private InputField inputChangeNote;

    private Workshop.Editor item;

    private void Awake()
    {
        if (SetManager.Info["id"] != null)
            item = SteamManager.Client.Workshop.EditItem((ulong)SetManager.Info["id"]);
        else
        {
            item = SteamManager.Client.Workshop.CreateItem
            (
                SteamManager.Client.AppId,
                Workshop.ItemType.Community
            );
            item.Publish();

            SetManager.Info["id"] = item.Id;
            SetManager.Info.Save();
        }
    }

    public void Upload()
    {
        item.Type = Workshop.ItemType.Community;
        item.Title = SetManager.Name;
        item.Description = RemoveTags((string)SetManager.Info["description"]);
        item.ChangeNote = inputChangeNote.text;
        item.Folder = SetManager.FullName;

        var imagePath = SetManager.FullName + "\\Image.png";
        if (File.Exists(imagePath))
            item.PreviewImage = imagePath;

        item.Publish();
    }

    private static string RemoveTags(string text)
    {
        text = text.Replace("<b>", "[b]");
        text = text.Replace("</b>", "[/b]");

        text = text.Replace("<i>", "[i]");
        text = text.Replace("</i>", "[/i]");

        text = text.Replace("</color>", string.Empty);

        return text;
    }
}