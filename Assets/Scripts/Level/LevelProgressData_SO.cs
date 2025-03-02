using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HybridPuzzle.SlinkyJam.Level
{
    [CreateAssetMenu(fileName = "LevelProgressData", menuName = "StickBlast/Level Progress Data", order = 1)]
    public class LevelProgressData_SO : ScriptableObject
    {
       [HideInInspector] public int currentLevelIndex = 0; // Mevcut seviye indexi
        public List<LevelData_SO> levels; // T�m seviyeler
    }
}