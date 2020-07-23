using UnityEngine;

namespace Sharp.Core
{
    public static class Constants
    {
        public const int CellLayer = 8;
        public const int UnitLayer = 9;
        public const int FogLayer = 10;

        public const int CellMask = 1 << CellLayer;
        public const int UnitMask = 1 << UnitLayer;
        public const int FogMask = 1 << FogLayer;

        public const float Time = .2f;

        public static readonly Vector2[] Directions =
        {
            Vector2.up,
            Vector2.right,
            Vector2.down,
            Vector2.left
        };

        public static readonly Vector3[] Eulers =
        {
            new Vector3(0, 0, 0),
            new Vector3(0, 0, -90),
            new Vector3(0, 0, -180),
            new Vector3(0, 0, -270)
        };

        public static readonly Quaternion[] Rotations =
        {
            Quaternion.Euler(Eulers[0]),
            Quaternion.Euler(Eulers[1]),
            Quaternion.Euler(Eulers[2]),
            Quaternion.Euler(Eulers[3])
        };

        public static readonly string Root = Application.isEditor ? "Build\\" : string.Empty;
        public static readonly string SetRoot = Root + "Sets\\";
        public static readonly string EditorRoot = Root + "Editor\\";
        public static readonly string SettingsRoot = Root + "Settings\\";
    }
}
