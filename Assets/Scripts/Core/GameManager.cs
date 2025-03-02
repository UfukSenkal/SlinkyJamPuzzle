using UnityEngine;
using HybridPuzzle.SlinkyJam.Grid;
using HybridPuzzle.SlinkyJam.Level;

namespace HybridPuzzle.SlinkyJam.Core
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [SerializeField] private GridManager gridManager;
        [SerializeField] private LevelData_SO levelData;

        private int remainingSlinkies;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        private void Start()
        {
            remainingSlinkies = levelData.slinkies.Count;
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
            // TODO: Buraya kazanma ekraný ekleyebilirsin
        }

        public void Fail()
        {
            Debug.Log("Game Over!");
            // TODO: Buraya kaybetme ekraný ekleyebilirsin
        }
    }
}
