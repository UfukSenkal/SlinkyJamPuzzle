using System.Collections.Generic;
using UnityEngine;
using HybridPuzzle.SlinkyJam.Slinky;
using HybridPuzzle.SlinkyJam.Helper;
using Unity.VisualScripting.Antlr3.Runtime.Misc;

namespace HybridPuzzle.SlinkyJam.Grid
{
    [System.Serializable]
    public class GridConfig
    {
        public Vector3 offset;
        public GameObject slotPrefab;
        private Dictionary<int, GridData> _slots = new Dictionary<int, GridData>();
        private Vector2Int size;

        public int SlotCount => size.x * size.y;

        public void InitializeGrid(Vector2Int size, Transform parent)
        {
            this.size = size;
            _slots.Clear();

            for (int y = 0; y < size.y; y++)
            {
                for (int x = 0; x < size.x; x++)
                {
                    int index = x + y * size.x;
                    Vector3 position = new Vector3(x + offset.x, offset.y, -y + offset.z);
                    _slots[index] = new GridData(null, position);

                    if (slotPrefab != null)
                    {
                        GameObject slot = Object.Instantiate(slotPrefab, position, Quaternion.identity, parent);
                        slot.name = $"Slot_{index}";
                    }
                }
            }
        }
        public SlinkyBehaviour GetSlinkyAt(int slotIndex)
        {
            return _slots[slotIndex].slinky;
        }

        public void PlaceSlinky(SlinkyBehaviour slinky, int slotIndex)
        {
            if (_slots.ContainsKey(slotIndex))
                _slots[slotIndex].slinky = slinky;
        }

        public void RemoveSlinky(int slotIndex)
        {
            if (_slots.ContainsKey(slotIndex))
                _slots[slotIndex].slinky = null;
        }
        public int GetFirstEmptySlotIndex()
        {
            foreach (var item in _slots)
            {
                if (item.Value.IsEmpty)
                    return item.Key;
            }
            return -1;
        }
        public bool IsSlotEmpty(int slotIndex)
        {
            return _slots.ContainsKey(slotIndex) && _slots[slotIndex].IsEmpty;
        }
        public bool IsGridFull()
        {
            foreach (var slot in _slots)
            {
                if (slot.Value.IsEmpty) return false;
            }
            return true;
        }

        public Vector3 GetWorldPosition(int slotIndex)
        {
            return _slots.ContainsKey(slotIndex) ? _slots[slotIndex].pos : Vector3.zero;
        }

        public bool ContainsSlotIndex(int slotIndex)
        {
            return _slots.ContainsKey(slotIndex);
        }

        public bool ContainsWorldPos(Vector3 worldPos)
        {
            Vector3 localPos = worldPos - offset;
            int x = Mathf.FloorToInt(localPos.x);
            int y = Mathf.FloorToInt(-localPos.z);
            return x >= 0 && x < size.x && y >= 0 && y < size.y;
        }

        public int GetSlotIndexFromWorldPos(Vector3 worldPos)
        {
            Vector3 localPos = worldPos - offset;
            int x = Mathf.FloorToInt(localPos.x);
            int y = Mathf.FloorToInt(-localPos.z);

            return (x >= 0 && x < size.x && y >= 0 && y < size.y) ? x + y * size.x : -1;
        }

        public List<SlinkyBehaviour> GetAllSlinkies()
        {
            List<SlinkyBehaviour> slinkies = new List<SlinkyBehaviour>();
            foreach (var kvp in _slots)
            {
                if (kvp.Value.slinky != null)
                {
                    slinkies.Add(kvp.Value.slinky);
                }
            }
            return slinkies;
        }
    }
}
