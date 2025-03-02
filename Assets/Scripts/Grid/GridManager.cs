using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using HybridPuzzle.SlinkyJam.Level;
using HybridPuzzle.SlinkyJam.Matching;
using HybridPuzzle.SlinkyJam.Slinky;
using HybridPuzzle.SlinkyJam.Helper;

namespace HybridPuzzle.SlinkyJam.Grid
{
    public class GridManager : MonoBehaviour, ILevelInitializer
    {
        [SerializeField] private GridConfig upperGridConfig;
        [SerializeField] private GridConfig lowerGridConfig;
        [SerializeField] private SlinkyBehaviour slinkyPrefab;

        private MatchManager _matchManager;
        private LevelData_SO _levelData;

        public GridConfig LowerGridConfig => lowerGridConfig;

        public void InitiliazeWithLevel(LevelData_SO currentLevel)
        {
            _levelData = currentLevel;
            InitializeGrids();
            SpawnSlinkies();
            _matchManager = new MatchManager(this);
        }
        private void InitializeGrids()
        {
            upperGridConfig.InitializeGrid(_levelData.upperGridSize, transform);
            lowerGridConfig.InitializeGrid(_levelData.lowerGridSize, transform);
        }

        private void SpawnSlinkies()
        {
            foreach (var slinkyData in _levelData.slinkies)
            {
                if (!upperGridConfig.ContainsSlotIndex(slinkyData.startSlot) || !upperGridConfig.ContainsSlotIndex(slinkyData.endSlot))
                {
                    Debug.LogError($"Invalid slot positions for slinky: {slinkyData.startSlot}, {slinkyData.endSlot}");
                    continue;
                }

                Vector3 startPos = upperGridConfig.GetWorldPosition(slinkyData.startSlot);
                Vector3 endPos = upperGridConfig.GetWorldPosition(slinkyData.endSlot);

                SlinkyBehaviour slinky = Instantiate(slinkyPrefab, startPos, Quaternion.identity);
                slinky.Initialize(startPos, endPos, slinkyData.color);
                upperGridConfig.PlaceSlinky(slinky, slinkyData.startSlot);
                upperGridConfig.PlaceSlinky(slinky, slinkyData.endSlot);
                slinky.SlotIndex = slinkyData.startSlot;
            }
        }

        public void PlaceSlinkyInLowerGrid(SlinkyBehaviour slinky)
        {
            int slotIndex = FindBestSlot(slinky);
            if (slotIndex == -1) return;

            ShiftSlinkiesRight(slotIndex);

            lowerGridConfig.PlaceSlinky(slinky, slotIndex);
            slinky.SlotIndex = slotIndex;
            slinky.MoveToTarget(lowerGridConfig.GetWorldPosition(slotIndex));
            slinky.onMovementComplete += () =>
            {
                _matchManager.CheckMatch();
                slinky.onMovementComplete = null;
                if (lowerGridConfig.IsGridFull())
                    Core.GameManager.Instance.Fail();
            };

        }

        private int FindBestSlot(SlinkyBehaviour slinky)
        {
            int bestSlot = -1;
            for (int i = 0; i < lowerGridConfig.SlotCount; i++)
            {
                if (lowerGridConfig.IsSlotEmpty(i))
                {
                    if (bestSlot == -1) bestSlot = i;
                }
                else if (lowerGridConfig.GetSlinkyAt(i).Color == slinky.Color)
                {
                    bestSlot = i + 1;
                }
            }
            return bestSlot < lowerGridConfig.SlotCount ? bestSlot : -1;
        }

        private void ShiftSlinkiesRight(int startIndex)
        {
            for (int i = lowerGridConfig.SlotCount - 1; i > startIndex; i--)
            {
                if (!lowerGridConfig.IsSlotEmpty(i - 1))
                {
                    SlinkyBehaviour slinkyToMove = lowerGridConfig.GetSlinkyAt(i - 1);
                    lowerGridConfig.RemoveSlinky(i - 1);
                    lowerGridConfig.PlaceSlinky(slinkyToMove, i);
                    slinkyToMove.MoveToTarget(lowerGridConfig.GetWorldPosition(i));
                    slinkyToMove.SlotIndex = i;
                }
            }
        }
        public void ShiftRemainingSlinkies()
        {
            List<SlinkyBehaviour> slinkies = LowerGridConfig.GetAllSlinkies();
            Queue<SlinkyBehaviour> shiftQueue = new Queue<SlinkyBehaviour>();

            for (int i = 0; i < slinkies.Count; i++)
            {
                if (slinkies[i] != null)
                {
                    shiftQueue.Enqueue(slinkies[i]);
                }
            }

            for (int i = 0; i < LowerGridConfig.SlotCount; i++)
            {
                if (shiftQueue.Count > 0)
                {
                    SlinkyBehaviour slinky = shiftQueue.Dequeue();
                    int newSlot = lowerGridConfig.GetFirstEmptySlotIndex();
                    lowerGridConfig.RemoveSlinky(slinky.SlotIndex);
                    LowerGridConfig.PlaceSlinky(slinky, newSlot);
                    slinky.SlotIndex = newSlot;
                    slinky.MoveToTarget(LowerGridConfig.GetWorldPosition(newSlot));
                }
            }
        }


    }
    
}
