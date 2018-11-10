using UnityEngine;

[CreateAssetMenu(menuName = "Actions/Blink")]
public class BlinkAction : BaseAction
{
    public override void Do(PlayerObject player)
    {
        var destination = player.Movable.IntPosition + 2 * Constants.Directions[player.Movable.Direction];
        if (MovableComponent.CanMove(destination))
            player.Movable.Position = destination;
    }
}