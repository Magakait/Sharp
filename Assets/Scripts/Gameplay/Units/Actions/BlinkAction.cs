using UnityEngine;

[CreateAssetMenu(menuName = "Actions/Blink")]
public class BlinkAction : BaseAction
{
    public override void Do(PlayerObject player) => 
        player.Movable.Position = player.Movable.IntPosition + 2 * Constants.Directions[player.Movable.Direction];
}