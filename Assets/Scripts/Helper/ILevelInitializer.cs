using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HybridPuzzle.SlinkyJam.Helper
{
    public interface ILevelInitializer
    {
        public void InitiliazeWithLevel(Level.LevelData_SO currentLevel);
    }
}
