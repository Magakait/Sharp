using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleScalerComponent : MonoBehaviour
{
    public Vector2 scalar;

    [Space(10)]
    public Transform referenceTransform;
    public bool global;

    private new ParticleSystem particleSystem;

    private int maxParticles;
    private float rateOverTime;
    private float rateOverDistance;
    private ParticleSystem.Burst[] bursts;

    private void Awake()
    {
        particleSystem = GetComponent<ParticleSystem>();

        maxParticles = particleSystem.main.maxParticles;
        rateOverTime = particleSystem.emission.rateOverTime.constant;
        rateOverDistance = particleSystem.emission.rateOverDistance.constant;

        bursts = new ParticleSystem.Burst[particleSystem.emission.burstCount];
        particleSystem.emission.GetBursts(bursts);
    }

    private void Start()
    {
        Scale();
    }

    public void Scale()
    {
        Vector2 scale = Vector2.Scale(global ? referenceTransform.lossyScale : referenceTransform.localScale, scalar);
        float multiplier = Mathf.Max(Mathf.Abs(scale.x), Mathf.Abs(scale.y), Mathf.Abs(scale.x * scale.y));

        ParticleSystem.MainModule main = particleSystem.main;
        main.maxParticles = (int)(maxParticles * multiplier);

        ParticleSystem.EmissionModule emission = particleSystem.emission;
        emission.rateOverTime = rateOverTime * multiplier;
        emission.rateOverDistance = rateOverDistance * multiplier;

        for (int i = 0; i < bursts.Length; i++)
            bursts[i].count = bursts[i].count.constant * multiplier;

        emission.SetBursts(bursts);

        for (int i = 0; i < bursts.Length; i++)
            bursts[i].count = bursts[i].count.constant / multiplier;

        particleSystem.Simulate(Time.time);
        particleSystem.Play();
    }

    public void Scale(Vector3 scale)
    {
        referenceTransform.localScale = scale;
        Scale();
    }
}
