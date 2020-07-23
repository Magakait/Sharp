using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json.Linq;

namespace Sharp.Editor.Widgets
{
    public class SequenceWidget : BaseWidget
    {
        [Space(10)]
        [SerializeField]
        private InputField input;

        private string sequence;
        public string Sequence
        {
            get => sequence;
            set
            {
                sequence = value
                    .Replace('↑', '0')
                    .Replace('→', '1')
                    .Replace('↓', '2')
                    .Replace('←', '3');
            }
        }

        private void Awake() =>
            input.onValidateInput += delegate (string text, int index, char last)
            {
                switch (last)
                {
                    case '1':
                        return '→';
                    case '2':
                        return '↓';
                    case '3':
                        return '←';
                    default:
                        return '↑';
                }
            };

        public void Loop() =>
            input.text = Sequence + new string
                (
                    Sequence
                        .Select(c => (char)((c - '0' + 2) % 4 + '0'))
                        .ToArray()
                );

        public void Mirror() =>
            input.text = Sequence + new string
                (
                    Sequence
                        .Select(c => (char)((c - '0' + 2) % 4 + '0'))
                        .Reverse()
                        .ToArray()
                );

        protected override void Read(JToken value, JToken attributes) =>
            input.text = (string)value;

        protected override JToken Write() =>
            Sequence;
    }
}
