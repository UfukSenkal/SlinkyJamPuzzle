using UnityEngine;
using HybridPuzzle.SlinkyJam.Grid;
using HybridPuzzle.SlinkyJam.Level;
using HybridPuzzle.SlinkyJam.Helper;

namespace HybridPuzzle.SlinkyJam.Core
{
    public class GameManager : MonoBehaviour, ILevelInitializer
    {
        public static GameManager Instance { get; private set; }

        private LevelData_SO _levelData;
        private int remainingSlinkies;

        public void InitiliazeWithLevel(LevelData_SO currentLevel)
        {
            Instance = this;
            _levelData = currentLevel;
            remainingSlinkies = _levelData.slinkies.Count;
        }


        public void OnMatchCompleted()
        {
            remainingSlinkies -= 3;
            if (remainingSlinkies <= 0)
            {
                Win();
            }
        }



        private void Win()
        {
            Debug.Log("You Win!");
            var successMenu = _levelData.LevelInstance.Get<UI.GameSuccessMenu>();
            successMenu.OpenMenu();
        }

        public void Fail()
        {
            Debug.Log("Game Over!");
            var gameOverMenu = _levelData.LevelInstance.Get<UI.GameOverMenu>();
            gameOverMenu.OpenMenu();
        }


    }
}
