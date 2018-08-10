using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleScalerComponent : MonoBehaviour
{
    public Vector2 scalar;

    [Space(10)]
    public Transform referenceTransform;
    public bool global;

    public ParticleSystem ParticleSystem { get; private set; }

    private int maxParticles;
    private float rateOverTime;
    private float rateOverDistance;
    private ParticleSystem.Burst[] bursts;

    private void Awake()
    {
        ParticleSystem = GetComponent<ParticleSystem>();

        maxParticles = ParticleSystem.main.maxParticles;
        rateOverTime = ParticleSystem.emission.rateOverTime.constant;
        rateOverDistance = ParticleSystem.emission.rateOverDistance.constant;

        bursts = new ParticleSystem.Burst[ParticleSystem.emission.burstCount];
        ParticleSystem.emission.GetBursts(bursts);
    }

    private void Start()
    {
        Scale();
    }

    public void Scale()
    {
        var scale = Vector2.Scale(global ? referenceTransform.lossyScale : referenceTransform.localScale, scalar);
        var multiplier = Mathf.Max(Mathf.Abs(scale.x), Mathf.Abs(scale.y), Mathf.Abs(scale.x * scale.y));

        var main = ParticleSystem.main;
        main.maxParticles = (int)(maxParticles * multiplier);

        var emission = ParticleSystem.emission;
        emission.rateOverTime = rateOverTime * multiplier;
        emission.rateOverDistance = rateOverDistance * multiplier;

        for (var i = 0; i < bursts.Length; i++)
            bursts[i].count = bursts[i].count.constant * multiplier;

        emission.SetBursts(bursts);

        for (var i = 0; i < bursts.Length; i++)
            bursts[i].count = bursts[i].count.constant / multiplier;

        ParticleSystem.Simulate(Time.time);
        ParticleSystem.Play();
    }

    public void Scale(Vector3 scale)
    {
        referenceTransform.localScale = scale;
        Scale();
    }
}
