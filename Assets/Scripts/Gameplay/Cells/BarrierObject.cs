using UnityEngine;
using Sharp.Core;

namespace Sharp.Gameplay
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(CellComponent))]
    [RequireComponent(typeof(AudioSource))]
    public class BarrierObject : MonoBehaviour
    {
        [SerializeField]
        private ParticleSystem haloEffect;

        private Animator animator;
        private CellComponent cell;
        private new AudioSource audio;

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
                if (Charges == 0)
                    audio.Play();
            }
        }

        private void Awake()
        {
            animator = GetComponent<Animator>();
            cell = GetComponent<CellComponent>();
            audio = GetComponent<AudioSource>();
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
