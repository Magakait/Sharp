﻿using UnityEngine;

using DG.Tweening;
using Newtonsoft.Json.Linq;

public class BarrierObject : SerializableObject
{
    [Space(10)]
    [SerializeField]
    private CellComponent cell;

    private int charges;
    public int Charges
    {
        get
        {
            return charges;
        }
        set
        {
            charges = value;

            cell.Hollowed = Charges > 0;
            animation[0].Play(Charges == 0);
            haloEffect.Emission(Charges > 0);
        }
    }

    private void Awake() =>
        animation = gameObject.AddComponent<TweenArrayComponent>().Init
        (
            DOTween.Sequence().Insert
            (
                cellTransform
                    .DOScale(0, Constants.Time)
            )
        );

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Charges > 0)
            collision.GetComponent<UnitComponent>().Kill();
    }

    #region animation

    [Header("Animation")]
    [SerializeField]
    private Transform cellTransform;
    [SerializeField]
    private ParticleSystem haloEffect;

    private new TweenArrayComponent animation;

    #endregion
}