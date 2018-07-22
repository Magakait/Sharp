using System.Collections;

using UnityEngine;

public class CheckpointObject : SerializableObject
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerObject player = collision.GetComponent<PlayerObject>();
        if (player && player.spawn != this)
            Activate(player);
    }

    #region gameplay

    private void Activate(PlayerObject player)
    {
        if (player.spawn)
            player.spawn.spire.Emission(false);

        player.spawn = this;
        player.spawn.spire.Emission(true);

        Instantiate(burst, transform.position, Quaternion.identity);
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
    [SerializeField]
    private ParticleSystem burst;

    #endregion
}