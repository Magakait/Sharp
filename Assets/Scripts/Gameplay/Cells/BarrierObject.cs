using UnityEngine;
using Sharp.Core;

namespace Sharp.Gameplay
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(CellComponent))]
    public class BarrierObject : MonoBehaviour
    {
        [SerializeField]
        private ParticleSystem haloEffect;

        private Animator animator;
        private CellComponent cell;

        private int charges;
        public int Charges
        {
            get => charges;
            set
            {
                charges = value;
                cell.Hollowed = Charges > 0;
                animator.SetBool("Open", Charges == 0);
                haloEffect.Emission(Charges > 0);
            }
        }

        private void Awake()
        {
            animator = GetComponent<Animator>();
            cell = GetComponent<CellComponent>();
        }

        private void Start() =>
            Charges = Charges;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (Charges > 0)
                collision.GetComponent<UnitComponent>().Kill();
        }
    }
}
