using UnityEngine;

namespace AlKaitagi.SharpCore.Painters
{
    [RequireComponent(typeof(ParticleSystem))]
    [AddComponentMenu("Painters/Particle Painter")]
    public class ParticlePainter : BasePainter
    {
        public override void Refresh()
        {
            var particleSystem = GetComponent<ParticleSystem>();

            var main = particleSystem.main;
            main.startColor = Fade(Variable.Value, main.startColor.color.a);

            if (particleSystem.isPlaying)
            {
                particleSystem.Clear();
                particleSystem.Stop();
                particleSystem.Play();
            }
        }
    }
}
