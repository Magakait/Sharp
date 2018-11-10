using UnityEngine;

[CreateAssetMenu(menuName = "Actions/Kick")]
public class KickAction : BaseAction
{
    public override void Do(PlayerObject player)
    {
        var target = PhysicsUtility.Raycast<MovableComponent>
        (
            player.Movable.Position + .15f * Constants.Directions[player.Movable.Direction],
            player.Movable.Direction,
            Constants.UnitMask
        );

        if (target && Vector2.Distance(player.Movable.Position, target.Position) <= 1.15f)
            target.Move(target.IntPosition + Constants.Directions[player.Movable.Direction]);
    }
}