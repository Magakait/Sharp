using System.Collections;
using UnityEngine;

public class CheckpointObject : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerObject player = collision.GetComponent<PlayerObject>();
        if (player)
            Activate(player);
    }

    #region gameplay

    private BaseAction action;
    private float cooldown;
    private BaseMovement movement;
    private float transition;

    private void Activate(PlayerObject player)
    {
        if (player.Checkpoint)
            player.Checkpoint.spire.Emission(false);

        action = player.Action;
        cooldown = player.Cooldown;
        movement = player.Movement;
        transition = player.Movable.Transition;

        player.Checkpoint = this;
        spire.Emission(true);
    }

    public IEnumerator Spawn()
    {
        yield return new WaitForSeconds(2 * Constants.Time);
        CameraManager.Move(transform.position);
        yield return new WaitForSeconds(2 * Constants.Time);

        var player = LevelManager.AddInstance("Player", transform.position).GetComponent<PlayerObject>();
        player.Action = action;
        player.Cooldown = cooldown;
        player.Movement = movement;
        player.Movable.Transition = transition;
    }

    #endregion

    #region animation

    [Space(10)]
    [SerializeField]
    private ParticleSystem spire;

    #endregion
}
