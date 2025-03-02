using System;
using UnityEngine;

namespace HybridPuzzle.SlinkyJam.Slinky
{
    public class SlinkySegment : MonoBehaviour
    {
        private SlinkyBehaviour _parentSlinky;

        internal void OnSegmentClicked()
        {
            OnMouseDown();
        }

        private void Awake()
        {
            _parentSlinky = GetComponentInParent<SlinkyBehaviour>();
        }

        private void OnMouseDown()
        {
            if (_parentSlinky != null)
            {
                _parentSlinky.OnSegmentClicked();
            }
        }
    }
}