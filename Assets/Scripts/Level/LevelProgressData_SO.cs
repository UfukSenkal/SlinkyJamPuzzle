using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HybridPuzzle.SlinkyJam.Level
{
    [CreateAssetMenu(fileName = "LevelProgressData", menuName = "SlinkyJam/Level Progress Data", order = 1)]
    public class LevelProgressData_SO : ScriptableObject
    {
       [HideInInspector] public int currentLevelIndex = 0;
       [HideInInspector] public int currentLevelCount = 0;
        public List<LevelData_SO> levels; 
    }
}