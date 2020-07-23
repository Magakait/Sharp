using UnityEngine;
using Sharp.Core;

namespace Sharp.Gameplay
{
    [CreateAssetMenu(menuName = "Actions/Switch")]
    public class SwitchAction : BaseAction
    {
        public override void Do(PlayerObject player)
        {
            var target = PhysicsUtility.Overlap<StateComponent>
            (
                player.Movable.IntPosition + Constants.Directions[player.Movable.Direction],
                Constants.CellMask
            );

            if (target)
                target.State++;
        }
    }
}
