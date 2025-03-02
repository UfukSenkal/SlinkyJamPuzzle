using HybridPuzzle.SlinkyJam.Slinky;
using System.Collections.Generic;
using UnityEngine;

namespace HybridPuzzle.SlinkyJam.Grid
{
    public class MatchManager
    {
        private GridManager _gridManager;

        public MatchManager(GridManager gridManager)
        {
            _gridManager = gridManager;
        }

        public void CheckMatch()
        {
            List<SlinkyBehaviour> lowerGridSlinkies = _gridManager.GetLowerGridSlinkies();

            for (int i = 0; i <= lowerGridSlinkies.Count - 3; i++)
            {
                if (lowerGridSlinkies[i] != null &&
                    lowerGridSlinkies[i + 1] != null &&
                    lowerGridSlinkies[i + 2] != null)
                {
                    if (lowerGridSlinkies[i].Color == lowerGridSlinkies[i + 1].Color &&
                        lowerGridSlinkies[i].Color == lowerGridSlinkies[i + 2].Color)
                    {
                        ResolveMatch(lowerGridSlinkies[i], lowerGridSlinkies[i + 1], lowerGridSlinkies[i + 2]);
                        return;
                    }
                }
            }
        }

        private void ResolveMatch(SlinkyBehaviour first, SlinkyBehaviour middle, SlinkyBehaviour last)
        {
            Vector3 targetPosition = _gridManager.GetSlotPosition(middle.SlotIndex);
            first.MoveToTarget(targetPosition);
            last.MoveToTarget(targetPosition);

            last.onMovementComplete = () => DestroyMatchedSlinkies(first, middle, last);
        }

        private void DestroyMatchedSlinkies(SlinkyBehaviour first, SlinkyBehaviour middle, SlinkyBehaviour last)
        {
            _gridManager.RemoveSlinkyFromGrid(first);
            _gridManager.RemoveSlinkyFromGrid(middle);
            _gridManager.RemoveSlinkyFromGrid(last);

            GameObject.Destroy(first.gameObject);
            GameObject.Destroy(middle.gameObject);
            GameObject.Destroy(last.gameObject);
        }
    }
}