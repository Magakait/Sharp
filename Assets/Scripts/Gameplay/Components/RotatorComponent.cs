using UnityEngine;

using DG.Tweening;

public class RotatorComponent : MonoBehaviour
{
    public float transition;

    private Tweener tweener;

    private void Awake() => tweener = transform.DORotate(Vector3.zero, transition);

    private void Start() => tweener.Complete();

    private void OnDestroy() => tweener.Kill();

    public void Rotate(int direction) => Rotate(Constants.Eulers[direction]);

    public void Rotate(Vector3 eulerAngles) =>
        tweener
            .ChangeValues(transform.eulerAngles, eulerAngles)
            .Restart();

}