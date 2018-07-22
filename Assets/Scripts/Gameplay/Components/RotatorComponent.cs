using UnityEngine;

using DG.Tweening;

public class RotatorComponent : MonoBehaviour
{
    public float transition;

    private Tweener tweener;

    private void Awake() => 
        tweener = transform.DORotate(Vector3.zero, transition);

    private void Start() => 
        tweener.Complete();

    private void OnDestroy() => 
        tweener.Kill();

    public void Rotate(int direction) => 
        tweener
            .ChangeValues(transform.eulerAngles, Constants.Eulers[direction])
            .Restart();
}