using UnityEngine;
using Sharp.Core;

namespace Sharp.Gameplay
{
    [CreateAssetMenu(menuName = "Actions/Push")]
    public class PushAction : BaseAction
    {
        public override void Do(PlayerObject player)
        {
            var target = PhysicsUtility.Raycast<MovableComponent>
            (
                player.Movable.Position + .1f * Constants.Directions[player.Movable.Direction],
                player.Movable.Direction,
                Constants.UnitMask,
                .6f
            );

            if (target && target.CanMove(player.Movable.Direction))
                target.Move(player.Movable.Direction);
        }
    }
}
