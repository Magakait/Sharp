using UnityEngine;

public class CameraPainter : BasePainter
{
    [SerializeField]
    private new Camera camera;

    public override void Refresh() =>
        camera.backgroundColor = Variable.Value;
}