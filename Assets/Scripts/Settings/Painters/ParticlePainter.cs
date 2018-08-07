using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticlePainter : BasePainter<ParticleSystem>
{
    public override void Refresh()
    {
        var main = component.main;
        main.startColor = Variable.Value.Fade(main.startColor.color.a);

        component.Refresh();
    }
}
