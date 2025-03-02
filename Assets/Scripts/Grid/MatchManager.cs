using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using HybridPuzzle.SlinkyJam.Grid;
using HybridPuzzle.SlinkyJam.Slinky;
using HybridPuzzle.SlinkyJam.Core;

namespace HybridPuzzle.SlinkyJam.Matching
{
    public class MatchManager
    {
        private GridManager _gridManager;
        private Queue<SlinkyBehaviour> _matchQueue = new Queue<SlinkyBehaviour>();
        private SlinkyBehaviour _targetSlinky;
        private List<int> matchedSlotIndices = new List<int>();

        private bool _isMatching; 

        public MatchManager(GridManager gridManager)
        {
            _gridManager = gridManager;
        }

        public void CheckMatch()
        {
            if (_isMatching) return; 

            List<SlinkyBehaviour> matchedSlinkies = FindMatch();
            if (matchedSlinkies.Count == 3)
            {
                _isMatching = true; 
                _targetSlinky = matchedSlinkies[1];
                matchedSlotIndices.Clear();

                foreach (var slinky in matchedSlinkies)
                {
                    matchedSlotIndices.Add(slinky.SlotIndex);
                }

                AnimateMatch(matchedSlinkies);
            }
        }

        private List<SlinkyBehaviour> FindMatch()
        {
            List<SlinkyBehaviour> slinkies = _gridManager.LowerGridConfig.GetAllSlinkies();
            for (int i = 0; i <= slinkies.Count - 3; i++)
            {
                if (slinkies[i].Color == slinkies[i + 1].Color && slinkies[i].Color == slinkies[i + 2].Color)
                {
                    return new List<SlinkyBehaviour> { slinkies[i], slinkies[i + 1], slinkies[i + 2] };
                }
            }
            return new List<SlinkyBehaviour>();
        }

        private void AnimateMatch(List<SlinkyBehaviour> matchedSlinkies)
        {
            _matchQueue.Clear();
            _matchQueue.Enqueue(matchedSlinkies[0]);
            _matchQueue.Enqueue(matchedSlinkies[2]);

            ProcessNextMatch();
        }

        private void ProcessNextMatch()
        {
            if (_matchQueue.Count == 0)
            {
                _targetSlinky.gameObject.SetActive(false);
                _gridManager.LowerGridConfig.RemoveSlinky(_targetSlinky.SlotIndex);

                ShiftSlinkiesAfterMatch();
                _isMatching = false;
                GameManager.Instance.OnMatchCompleted();
                return;
            }

            SlinkyBehaviour slinky = _matchQueue.Dequeue();
            Vector3 targetPos = _gridManager.LowerGridConfig.GetWorldPosition(_targetSlinky.SlotIndex);
            slinky.MoveToTarget(targetPos);
            slinky.onMovementComplete += () =>
            {
                _gridManager.LowerGridConfig.RemoveSlinky(slinky.SlotIndex);
                slinky.gameObject.SetActive(false);
                ProcessNextMatch();
                slinky.onMovementComplete = null;
            };
        }

        private void ShiftSlinkiesAfterMatch()
        {
            _gridManager.ShiftRemainingSlinkies();
        }
    }
}
