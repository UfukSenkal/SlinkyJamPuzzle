using System.Collections.Generic;
using UnityEngine;

namespace HybridPuzzle.SlinkyJam.Helper
{
    public static class SlinkyColorHelper
    {
        private static readonly Dictionary<SlinkyColor, Color> _colorMap = new()
    {
        { SlinkyColor.Red, Color.red },
        { SlinkyColor.Blue, Color.blue },
        { SlinkyColor.Green, Color.green },
        { SlinkyColor.Yellow, Color.yellow },
        { SlinkyColor.Purple, new Color(0.5f, 0f, 0.5f) } // Mor renk
    };

        public static Color GetColor(SlinkyColor color) => _colorMap[color];
    }
    public enum SlinkyColor
    {
        Red,
        Blue,
        Green,
        Yellow,
        Purple
    }

}