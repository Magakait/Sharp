using System.Collections.Generic;
using UnityEngine;

namespace AlKaitagi.SharpUI
{
    public class CanvasRouter : MonoBehaviour
    {
        [SerializeField]
        private int index = 0;
        public int Index
        {
            get => index;
            set
            {
                if (Index >= 0 && Index < canvases.Length)
                    canvases[Index].Visible = false;
                index = value;
                if (Index >= 0 && Index < canvases.Length)
                    canvases[Index].Visible = true;
            }
        }
        [SerializeField]
        public CanvasToggle[] canvases = null;

        private Stack<int> history = new Stack<int>();

        private void Start() => Index = Index;

        public void Move(int index) =>
            history.Push(Index = index);

        public void Back()
        {
            if (history.Count > 0)
                Index = history.Pop();
        }
    }
}
