using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Sharp.Core;
using Sharp.UI;
using Sharp.Managers;
using Sharp.Camera;
using Newtonsoft.Json.Linq;

namespace Sharp.Gameplay
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(AudioSource))]
    public class EntranceObject : MonoBehaviour, ISerializable
    {
        private Animator animator;
        private new AudioSource audio;

        public bool Passed { get; private set; }

        private void Awake()
        {
            animator = GetComponent<Animator>();
            audio = GetComponent<AudioSource>();
        }

        private void Start()
        {
            if ((string)SetManager.Meta["selected"] == Level)
                CameraManager.Position = transform.position;

            if (Connected == 0 && Threshold > 0)
                gameObject.SetActive(false);
            else if (Connected > 0 && Connected < Threshold)
                canvas.gameObject.SetActive(false);
            else if (Passed)
                coreEffect.Emission(true);

            enterButton.interactable = SetManager.Levels.Contains(Level);
        }

        private void LateUpdate()
        {
            var mouse = CameraManager.WorldMouse;
            var distance = ((Vector2)transform.position - mouse).sqrMagnitude;

            var hover = distance <= 1;
            animator.SetBool("Hover", hover);

            if (hover
                && !UIUtility.IsOverUI
                && Mouse.current.leftButton.wasPressedThisFrame)
            {
                audio.Play();
                SetManager.Meta["selected"] = Level;
                SetManager.Meta.Save();
                CameraManager.Move(transform.position);
            }
        }

        public void Enter()
        {
            LevelManager.Load(Level);
            UIUtility.Main.LoadScene("Play");
        }

        public void Connect(EntranceObject target)
        {
            target.Connected++;

            var line = Instantiate(connectionLine, connectionLine.transform.parent);

            var position = (Vector2)transform.position;
            var destination = (Vector2)target.transform.position;
            var offset = .65f * (destination - position).normalized;

            line.SetPosition(0, position + offset);
            line.SetPosition(1, .5f * (position + destination));
            line.SetPosition(2, destination - offset);
        }

        [Header("Animation")]
        [SerializeField]
        private ParticleSystem coreEffect;
        [SerializeField]
        private LineRenderer connectionLine;

        [Space(10)]
        [SerializeField]
        private Canvas canvas;
        [SerializeField]
        private Text titleText;
        [SerializeField]
        private Button enterButton;

        public string Level
        {
            get => titleText.text;
            private set => titleText.text = value;
        }
        public int Threshold { get; private set; }
        public int Connected { get; private set; }
        public string Connections { get; private set; }

        public void Serialize(JToken token)
        {
            token["level"] = Level;
            token["threshold"] = Threshold;
            token["connections"] = Connections;
        }

        public void Deserialize(JToken token)
        {
            Level = (string)token["level"];
            Passed = SetManager.Meta["passed"].Any(t => (string)t == Level);

            Threshold = (int)token["threshold"];
            Connections = (string)token["connections"];
        }
    }
}
