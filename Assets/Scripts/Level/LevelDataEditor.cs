using HybridPuzzle.SlinkyJam.Helper;
using UnityEditor;
using UnityEngine;

namespace HybridPuzzle.SlinkyJam.Level
{
    [CustomEditor(typeof(LevelData_SO))]
    public class LevelDataEditor : Editor
    {
        private LevelData_SO _levelData;
        private int _selectedStartSlot = -1;
        private int _selectedEndSlot = -1;
        private float cellSize = 40.0f;
        private float buttonWidth = 40.0f;

        private void OnEnable()
        {
            _levelData = (LevelData_SO)target;
        }

        public override void OnInspectorGUI()
        {
            DrawGrid();
            GUILayout.Space(10);

            GUILayout.Label("Select Grid Slot", EditorStyles.boldLabel);
            GUILayout.Space(10);
            DrawDefaultInspector();

            if (_selectedStartSlot != -1)
            {
                GUILayout.Label($"Start Slot: {_selectedStartSlot}");
            }
            if (_selectedEndSlot != -1)
            {
                GUILayout.Label($"End Slot: {_selectedEndSlot}");
            }
        }

        private void DrawGrid()
        {
            if (_levelData == null) return;

            Vector2Int gridSize = _levelData.upperGridSize;

            GUILayout.BeginVertical();
            for (int y = 0; y < gridSize.y; y++)
            {
                GUILayout.BeginHorizontal();
                for (int x = 0; x < gridSize.x; x++)
                {
                    int slotIndex = GetSlotIndex(x, y);
                    string slotLabel = $"{slotIndex}";

                    SlinkyData slinky = _levelData.slinkies.Find(s => s.startSlot == slotIndex || s.endSlot == slotIndex);
                    Color buttonColor = slinky != null ? GetColor(slinky.color) : Color.white;

                    GUI.backgroundColor = buttonColor;
                    if (GUILayout.Button(slotLabel, GUILayout.Width(buttonWidth), GUILayout.Height(cellSize)))
                    {
                        HandleSlotClick(slotIndex);
                    }
                    GUI.backgroundColor = Color.white;

                    if (slinky != null)
                    {
                        Rect buttonRect = GUILayoutUtility.GetLastRect();
                        Rect labelRect = new Rect(buttonRect.xMax - 40, buttonRect.yMax - 40, 20, 20);

                        GUI.Label(labelRect, _levelData.slinkies.IndexOf(slinky).ToString(), EditorStyles.boldLabel);
                    }
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
        }

        private void HandleSlotClick(int slotIndex)
        {
            if (_selectedStartSlot == -1)
            {
                _selectedStartSlot = slotIndex;
            }
            else if (_selectedEndSlot == -1)
            {
                _selectedEndSlot = slotIndex;
                AddSlinkyToLevel(_selectedStartSlot, _selectedEndSlot);

                _selectedStartSlot = -1;
                _selectedEndSlot = -1;
            }
        }

        private void AddSlinkyToLevel(int startSlot, int endSlot)
        {
            Undo.RegisterCompleteObjectUndo(_levelData, "Add Slinky");

            _levelData.slinkies.Add(new SlinkyData
            {
                startSlot = startSlot,
                endSlot = endSlot,
                color = SlinkyColor.Red,
            });

            EditorUtility.SetDirty(_levelData);
        }

        private int GetSlotIndex(int x, int y)
        {
            return x + y * _levelData.upperGridSize.x;
        }

        private Color GetColor(SlinkyColor color)
        {
            return color switch
            {
                SlinkyColor.Red => Color.red,
                SlinkyColor.Blue => Color.blue,
                SlinkyColor.Green => Color.green,
                SlinkyColor.Yellow => Color.yellow,
                _ => Color.white,
            };
        }
    }
}
