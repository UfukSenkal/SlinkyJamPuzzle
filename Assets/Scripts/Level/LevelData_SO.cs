using UnityEngine;
using System.Collections.Generic;
using HybridPuzzle.SlinkyJam.Helper;

namespace HybridPuzzle.SlinkyJam.Level
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "SlinkyJam/LevelData")]
    public class LevelData_SO : ScriptableObject
    {
        public Vector2Int upperGridSize;
        public Vector2Int lowerGridSize = new Vector2Int(5, 1);
        public List<SlinkyData> slinkies;

        public LevelPlayer LevelInstance { get; private set; }
        public void InitiliazeLevel()
        {
            GameObject levelGO = new GameObject("Level - " + name);
            LevelInstance = levelGO.AddComponent<LevelPlayer>();
        }
        public void DestroyLevel()
        {
            if (LevelInstance != null)
                Object.Destroy(LevelInstance.gameObject);

        }

      
    }
}