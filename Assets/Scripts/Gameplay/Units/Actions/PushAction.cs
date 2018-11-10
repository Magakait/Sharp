using UnityEngine;

[CreateAssetMenu(menuName = "Actions/Push")]
public class PushAction : BaseAction
{
    public override void Do(PlayerObject player)
    {
        var target = PhysicsUtility.Raycast<MovableComponent>
        (
            player.Movable.Position + .15f * Constants.Directions[player.Movable.Direction],
            player.Movable.Direction,
            Constants.UnitMask,
            1.15f
        );

        if (target && target.CanMove(player.Movable.Direction))
            target.Move(player.Movable.Direction);
    }
}