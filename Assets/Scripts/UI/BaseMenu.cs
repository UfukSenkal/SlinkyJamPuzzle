using HybridPuzzle.SlinkyJam.Helper;
using HybridPuzzle.SlinkyJam.Level;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HybridPuzzle.SlinkyJam.UI
{
    public abstract class BaseMenu : MonoBehaviour, ILevelInitializer
    {
        [SerializeField] private Transform menuVisual;
        public abstract void InitiliazeWithLevel(LevelData_SO currentLevel);


        public void OpenMenu()
        {
            menuVisual.gameObject.SetActive(true);
        }
        public void CloseMenu()
        {
            menuVisual.gameObject.SetActive(false);
        }
    }
}