using HybridPuzzle.SlinkyJam.Level;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HybridPuzzle.SlinkyJam.UI {
    public class GameSuccessMenu : BaseMenu
    {
        [SerializeField] private Button nextLevelButton;
        public override void InitiliazeWithLevel(LevelData_SO currentLevel)
        {
            nextLevelButton.onClick.RemoveListener(OnNextLevelButtonClick);
            nextLevelButton.onClick.AddListener(OnNextLevelButtonClick);
            CloseMenu();
        }

        private void OnNextLevelButtonClick()
        {
            LevelManager.Instance.LoadNextLevel();
        }
    }
}
