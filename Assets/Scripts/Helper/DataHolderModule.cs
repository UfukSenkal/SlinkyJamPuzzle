using HybridPuzzle.SlinkyJam.Slinky;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HybridPuzzle.SlinkyJam.Helper
{
    public class GridData
    {
        public SlinkyBehaviour slinky;
        public Vector3 pos;

        public bool IsEmpty => slinky == null;
        public GridData()
        {
            slinky = null;
            pos = Vector3.zero;
        }
        public GridData(SlinkyBehaviour slinky, Vector3 pos)
        {
            this.slinky = slinky;
            this.pos = pos;
        }
    }

    [System.Serializable]
    public class SlinkyData
    {
        public int startSlot;
        public int endSlot;
        public SlinkyColor color;
    }
}
