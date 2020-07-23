﻿using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Sharp.Core;
using Newtonsoft.Json.Linq;
using DG.Tweening;

namespace Sharp.Gameplay
{
    public class WatcherObject : MonoBehaviour, ISerializable
    {
        private void Start() =>
            animation = gameObject.AddComponent<TweenContainer>().Init
            (
                DOTween.Sequence().Insert
                (
                    delayTransform
                        .DOScale(1, delay),
                    spikesTransform
                        .DOScale(1, Constants.Time)
                )
                    .OnComplete(() => Explode())
            );

        private void FixedUpdate()
        {
            Cast();
            animation[0].Play(!units.FirstOrDefault(unit => unit.GetComponent<PlayerObject>()));
        }

        #region gameplay

        [Header("Gameplay")]
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

        private static readonly List<UnitComponent> units = new List<UnitComponent>();

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

        #endregion

        #region animation

        [Header("Animation")]
        [SerializeField]
        private Transform spikesTransform;
        [SerializeField]
        private Transform delayTransform;
        [SerializeField]
        private Transform maskTransform;

        [Space(10)]
        [SerializeField]
        private ParticleSystem burstParticle;

        private new TweenContainer animation;

        #endregion

        #region serialization

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

        #endregion
    }
}
