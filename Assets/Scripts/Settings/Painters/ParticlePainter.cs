using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
[AddComponentMenu("Painters/Particle Painter")]
public class ParticlePainter : BasePainter
{
    public override void Refresh()
    {
        var particleSystem = GetComponent<ParticleSystem>();

        var main = particleSystem.main;
        main.startColor = Variable.Value.Fade(main.startColor.color.a);

        particleSystem.Refresh();
    }
}
