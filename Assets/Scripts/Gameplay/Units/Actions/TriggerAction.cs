using UnityEngine;

[CreateAssetMenu(menuName = "Actions/Trigger")]
public class TriggerAction : BaseAction
{
    public override void Do(PlayerObject player)
    {
        player.Collider.enabled = false;
        player.Collider.enabled = true;
    }
}