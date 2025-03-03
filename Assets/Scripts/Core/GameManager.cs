using UnityEngine;
using HybridPuzzle.SlinkyJam.Grid;
using HybridPuzzle.SlinkyJam.Level;
using HybridPuzzle.SlinkyJam.Helper;

namespace HybridPuzzle.SlinkyJam.Core
{
    public class GameManager : MonoBehaviour, ILevelInitializer
    {
        public static GameManager Instance { get; private set; }

        private GridManager _gridManager;

        private LevelData_SO _levelData;
        private int remainingSlinkies;

        public void InitiliazeWithLevel(LevelData_SO currentLevel)
        {
            Instance = this;
            _levelData = currentLevel;
            _gridManager = currentLevel.LevelInstance.Get<GridManager>();
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
            LevelManager.Instance.LoadNextLevel();
        }

        public void Fail()
        {
            Debug.Log("Game Over!");
            LevelManager.Instance.RestartLevel();
        }


    }
}
