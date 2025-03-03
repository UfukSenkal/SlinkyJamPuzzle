using HybridPuzzle.SlinkyJam.Helper;
using HybridPuzzle.SlinkyJam.Level;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HybridPuzzle.SlinkyJam.UI
{
    public class GameOverMenu : BaseMenu
    {
        [SerializeField] private Button restartLevelButton;
        public override void InitiliazeWithLevel(LevelData_SO currentLevel)
        {
            restartLevelButton.onClick.RemoveListener(OnNextLevelButtonClick);
            restartLevelButton.onClick.AddListener(OnNextLevelButtonClick);
            CloseMenu();
        }

        private void OnNextLevelButtonClick()
        {
            LevelManager.Instance.RestartLevel();
        }
    }
}
