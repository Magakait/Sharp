using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Sharp.Core;
using Newtonsoft.Json.Linq;

namespace Sharp.Gameplay
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(AudioSource))]
    public class WatcherObject : MonoBehaviour, ISerializable
    {
        [SerializeField]
        private int distance;
        public int Distance
        {
            get => distance;
            set
            {
                distance = value;
                maskTransform.localScale = 2 * Distance * Vector3.one;
            }
        }
        [SerializeField]
        private float delay;

        [Space(10)]
        [SerializeField]
        private Transform maskTransform;
        [SerializeField]
        private Transform delayTransform;
        [SerializeField]
        private ParticleSystem burstParticle;

        private static readonly List<UnitComponent> units = new List<UnitComponent>();
        private Animator animator;
        private new AudioSource audio;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            audio = GetComponent<AudioSource>();
        }

        private void FixedUpdate()
        {
            Cast();

            var scale = delayTransform.localScale.x;
            var step = Time.fixedDeltaTime / delay;

            var watching = units.FirstOrDefault(unit => unit.GetComponent<PlayerObject>());
            animator.SetBool("Watching", watching);
            if (!watching)
                step *= -1;

            scale = Mathf.Clamp01(scale + step);
            audio.volume = scale * 0.5f;
            delayTransform.localScale = scale * Vector2.one;
            if (scale == 1)
                Explode();
        }

        private void Cast() =>
            PhysicsUtility.OverlapBox
            (
                units,
                transform.position,
                maskTransform.localScale,
                Constants.UnitMask
            );

        public void Explode()
        {
            Cast();
            foreach (var unit in units.Where(unit => !unit.Killed && !unit.Virus))
                unit.Kill();

            Instantiate(burstParticle, maskTransform);
        }

        public void Serialize(JToken token)
        {
            token["distance"] = Distance;
            token["delay"] = delay;
        }

        public void Deserialize(JToken token)
        {
            Distance = (int)token["distance"];
            delay = (float)token["delay"];
        }
    }
}
