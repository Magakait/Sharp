using System.Collections;
using Sharp.Core;
using Sharp.Managers;
using Sharp.Camera;
using UnityEngine;

namespace Sharp.Gameplay
{
    [RequireComponent(typeof(AudioSource))]
    public class CheckpointObject : MonoBehaviour
    {
        [SerializeField]
        private ParticleSystem spire;

        private new AudioSource audio;

        private BaseAction action;
        private float cooldown;
        private BaseMovement movement;
        private float transition;

        private void Awake() =>
            audio = GetComponent<AudioSource>();

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponent<PlayerObject>() is PlayerObject pl)
            {
                if (pl.Checkpoint != this)
                    audio.Play();
                Activate(pl);
            }
        }

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
            yield return new WaitForSeconds(2 * .2f);
            CameraManager.Move(transform.position);
            yield return new WaitForSeconds(2 * .2f);

            var player = LevelManager.AddInstance("Player", transform.position).GetComponent<PlayerObject>();
            player.Action = action;
            player.Cooldown = cooldown;
            player.Movement = movement;
            player.Movable.Transition = transition;
        }
    }
}
