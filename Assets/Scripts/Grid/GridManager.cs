using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Drawing;
using HybridPuzzle.SlinkyJam.Slinky;
using HybridPuzzle.SlinkyJam.Level;

namespace HybridPuzzle.SlinkyJam.Grid
{
    public class GridManager : MonoBehaviour
    {
        [SerializeField] private LevelData levelData;
        [SerializeField] private GridConfig upperGridConfig; // Üstteki grid
        [SerializeField] private GridConfig lowerGridConfig; // Alttaki grid
        [SerializeField] private SlinkyBehaviour slinkyPrefab;

        private MatchManager _matchManager;

        private void Start()
        {
            InitializeGrids();
            SpawnSlinkies();
            _matchManager = new MatchManager(this);
        }

        private void InitializeGrids()
        {
            // Üstteki ve alttaki grid'leri initialize et
            upperGridConfig.InitializeGrid(levelData.upperGridSize, transform);
            lowerGridConfig.InitializeGrid(levelData.lowerGridSize, transform);
        }

        private void SpawnSlinkies()
        {
            foreach (var slinkyData in levelData.slinkies)
            {
                // Slot index'lerinin geçerli olup olmadýðýný kontrol et
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
        public int FindEmptySlotInLowerGrid()
        {
            for (int i = 0; i < lowerGridConfig.SlotCount; i++)
            {
                if (IsSlotOccupied(lowerGridConfig, i))
                {
                    return i;
                }
            }
            return -1; // Boþ slot bulunamadý
        }

        public bool IsSlotOccupied(GridConfig grid, int slotIndex)
        {
            return grid.ContainsSlotIndex(slotIndex) && grid.IsSlotEmpty(slotIndex);
        }

        public void PlaceSlinkyInLowerGrid(SlinkyBehaviour slinky)
        {
            lowerGridConfig.PlaceSlinky(slinky, slinky.SlotIndex);
            slinky.onMovementComplete -= OnSlinkyPlacedLowerGrid;
            slinky.onMovementComplete += OnSlinkyPlacedLowerGrid;
        }
        private void OnSlinkyPlacedLowerGrid()
        {
            _matchManager.CheckMatch();
        }
        public List<SlinkyBehaviour> GetLowerGridSlinkies()
        {
            List<SlinkyBehaviour> slinkyList = new List<SlinkyBehaviour>();
            var en = lowerGridConfig.GetSlinkyEnumerator();
            while (en.MoveNext())
            {
                slinkyList.Add(en.Current);
            }
            en.Dispose();

            return slinkyList;
        }
        public void RemoveSlinkyFromGrid(SlinkyBehaviour slinky, bool isLower = true, int slotIndex = 0)
        {
            if (isLower)
                lowerGridConfig.RemoveSlinky(slinky.SlotIndex);
            else
                upperGridConfig.RemoveSlinky(slotIndex);
        }

        public Vector3 GetSlotPosition(int slotIndex)
        {
            return lowerGridConfig.GetWorldPosition(slotIndex);
        }

        public GridConfig GetUpperGridConfig()
        {
            return upperGridConfig;
        }

        public GridConfig GetLowerGridConfig()
        {
            return lowerGridConfig;
        }
    }

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

}
