using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
[AddComponentMenu("Painters/Particle Painter")]
public class ParticlePainter : BasePainter
{
    public override void Refresh()
    {
        ParticleSystem particleSystem = GetComponent<ParticleSystem>();

        ParticleSystem.MainModule main = particleSystem.main;
        main.startColor = Variable.Value.Fade(main.startColor.color.a);

        particleSystem.Refresh();
    }
}
