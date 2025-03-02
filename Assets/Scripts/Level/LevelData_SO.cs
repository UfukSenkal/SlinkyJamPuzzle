using UnityEngine;
using System.Collections.Generic;
using HybridPuzzle.SlinkyJam.Helper;

namespace HybridPuzzle.SlinkyJam.Level
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "Game/LevelData")]
    public class LevelData_SO : ScriptableObject
    {
        public Vector2Int upperGridSize;
        public Vector2Int lowerGridSize = new Vector2Int(5, 1);
        public List<SlinkyData> slinkies;

        public GameObject LevelInstance { get; private set; }
        public void InitiliazeLevel()
        {
            GameObject levelGO = new GameObject("Level - " + name);
            LevelInstance = levelGO;
        }
        public void DestroyLevel()
        {
            if (LevelInstance != null)
                Object.Destroy(LevelInstance);

        }

      
    }
}