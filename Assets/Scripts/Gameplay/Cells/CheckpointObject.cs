using System.Collections;

using UnityEngine;

public class CheckpointObject : SerializableObject
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerObject player = collision.GetComponent<PlayerObject>();
        if (player && player.Checkpoint != this)
            Activate(player);
    }

    #region gameplay

    private void Activate(PlayerObject player)
    {
        if (player.Checkpoint)
            player.Checkpoint.spire.Emission(false);

        player.Checkpoint = this;
        player.Checkpoint.spire.Emission(true);
    }

    public IEnumerator Spawn()
    {
        yield return new WaitForSeconds(2 * Constants.Time);
        CameraManager.Move(transform.position);
        yield return new WaitForSeconds(2 * Constants.Time);
        Activate(LevelManager.Main.AddInstance(0, transform.position).GetComponent<PlayerObject>());
    }

    #endregion

    #region animation

    [Space(10)]
    [SerializeField]
    private ParticleSystem spire;

    #endregion
}