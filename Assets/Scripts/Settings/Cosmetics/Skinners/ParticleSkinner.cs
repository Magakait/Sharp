using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
[AddComponentMenu("Skinners/Particle Skinner")]
public class ParticleSkinner : BaseCosmetic<SpriteVariable>
{
    public override void Refresh()
    {
        ParticleSystem particleSystem = GetComponent<ParticleSystem>();

        ParticleSystem.TextureSheetAnimationModule animation = particleSystem.textureSheetAnimation;
        animation.SetSprite(0, Variable);

        particleSystem.Refresh();
    }
}