using UnityEngine;
using System.Collections.Generic;

namespace HybridPuzzle.SlinkyJam.Level
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "Game/LevelData")]
    public class LevelData : ScriptableObject
    {
        public Vector2Int upperGridSize;
        public Vector2Int lowerGridSize = new Vector2Int(5, 1); 
        public List<SlinkyData> slinkies;
    }

    [System.Serializable]
    public class SlinkyData
    {
        public int startSlot;
        public int endSlot;
        public Color color;
    }
}