using HybridPuzzle.SlinkyJam.Grid;
using HybridPuzzle.SlinkyJam.Helper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HybridPuzzle.SlinkyJam.Level
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance { get; private set; }

        public LevelProgressData_SO progressData;
        public GameObject loadingScreen;
        [SerializeField,LevelInitializer] public Component[] initializers;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        private void Start()
        {
            StartCoroutine(LoadLevelRoutine());
        }

        public void LoadNextLevel()
        {
            DestroyOldLevel();
            progressData.currentLevelIndex = (progressData.currentLevelIndex + 1) % progressData.levels.Count;
            StartCoroutine(LoadLevelRoutine());
        }
        public void RestartLevel()
        {
            DestroyOldLevel();
            StartCoroutine(LoadLevelRoutine());
        }
        private IEnumerator LoadLevelRoutine()
        {
            ShowLoadingScreen(true);
            yield return new WaitForSeconds(1);
            LoadCurrentLevel();

            ShowLoadingScreen(false);
        }

        private void LoadCurrentLevel()
        {
            if (progressData.currentLevelIndex >= progressData.levels.Count)
            {
                progressData.currentLevelIndex = 0;
            }

            LevelData_SO currentLevel = GetCurrentLevel();
            if (currentLevel != null)
            {
                currentLevel.InitiliazeLevel();
                foreach (var initializer in initializers)
                {
                    if (initializer is ILevelInitializer levelInitializer)
                    {

                        levelInitializer.InitiliazeWithLevel(currentLevel);
                        currentLevel.LevelInstance.Register(initializer);
                    }
                }
            }
        }

        private LevelData_SO GetCurrentLevel()
        {
            if (progressData.currentLevelIndex < progressData.levels.Count)
            {
                return progressData.levels[progressData.currentLevelIndex];
            }
            else
            {
                Debug.LogError("Level index out of range.");
                return null;
            }
        }

        private void ShowLoadingScreen(bool show)
        {

            if (loadingScreen != null)
                loadingScreen.SetActive(show);
        }
        private void DestroyOldLevel()
        {
            LevelData_SO currentLevel = GetCurrentLevel();
            if (currentLevel != null)
            {
                currentLevel.DestroyLevel();
            }
        }
    }
}
