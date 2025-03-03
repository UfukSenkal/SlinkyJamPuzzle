using HybridPuzzle.SlinkyJam.Helper;
using HybridPuzzle.SlinkyJam.Level;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HybridPuzzle.SlinkyJam.UI
{
    public class GameHud : BaseMenu
    {
        [SerializeField] private TMPro.TextMeshProUGUI levelText;

        public override void InitiliazeWithLevel(LevelData_SO currentLevel)
        {
            levelText.text = $"Level - {LevelManager.Instance.CurrentLevelCount}";
        }
    }
}
