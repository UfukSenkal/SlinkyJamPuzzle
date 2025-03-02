#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
namespace HybridPuzzle.SlinkyJam.Level
{

    [CustomEditor(typeof(LevelData))]
    public class LevelDataEditor : Editor
    {
        private LevelData _levelData;
        private const float CellSize = 20f; 
        private const float Padding = 5f; 

        private void OnEnable()
        {
            _levelData = (LevelData)target;
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("Grid Settings", EditorStyles.boldLabel);
            _levelData.upperGridSize = EditorGUILayout.Vector2IntField("Upper Grid Size", _levelData.upperGridSize);

            DrawGrid();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Slinkies", EditorStyles.boldLabel);
            for (int i = 0; i < _levelData.slinkies.Count; i++)
            {
                EditorGUILayout.BeginVertical("box");

                EditorGUILayout.LabelField($"Slinky {i + 1}");
                _levelData.slinkies[i].startSlot = EditorGUILayout.IntField("Start Slot", _levelData.slinkies[i].startSlot);
                _levelData.slinkies[i].endSlot = EditorGUILayout.IntField("End Slot", _levelData.slinkies[i].endSlot);
                _levelData.slinkies[i].color = EditorGUILayout.ColorField("Color", _levelData.slinkies[i].color);

                if (GUILayout.Button("Remove Slinky"))
                {
                    _levelData.slinkies.RemoveAt(i);
                    break;
                }

                EditorGUILayout.EndVertical();
            }

            if (GUILayout.Button("Add Slinky"))
            {
                _levelData.slinkies.Add(new SlinkyData());
            }

            if (GUI.changed)
            {
                EditorUtility.SetDirty(_levelData);
            }
        }

        private void DrawGrid()
        {
            EditorGUILayout.LabelField("Grid Preview");
            Rect gridRect = GUILayoutUtility.GetRect(
                CellSize * _levelData.upperGridSize.x + Padding * (_levelData.upperGridSize.x - 1),
                CellSize * _levelData.upperGridSize.y + Padding * (_levelData.upperGridSize.y - 1)
            );

            Handles.color = Color.gray;
            for (int x = 0; x <= _levelData.upperGridSize.x; x++)
            {
                float xPos = gridRect.x + x * (CellSize + Padding);
                Handles.DrawLine(new Vector3(xPos, gridRect.y, 0), new Vector3(xPos, gridRect.y + gridRect.height, 0));
            }
            for (int y = 0; y <= _levelData.upperGridSize.y; y++)
            {
                float yPos = gridRect.y + y * (CellSize + Padding);
                Handles.DrawLine(new Vector3(gridRect.x, yPos, 0), new Vector3(gridRect.x + gridRect.width, yPos, 0));
            }

            Handles.color = Color.red;
            foreach (var slinky in _levelData.slinkies)
            {
                Vector2 startPos = GetGridPosition(slinky.startSlot);
                Vector2 endPos = GetGridPosition(slinky.endSlot);

                Handles.DrawLine(
                    new Vector3(gridRect.x + startPos.x * (CellSize + Padding), gridRect.y + startPos.y * (CellSize + Padding), 0),
                    new Vector3(gridRect.x + endPos.x * (CellSize + Padding), gridRect.y + endPos.y * (CellSize + Padding), 0)
                );
            }
        }

        private Vector2 GetGridPosition(int slotIndex)
        {
            int x = slotIndex % _levelData.upperGridSize.x;
            int y = slotIndex / _levelData.upperGridSize.x;
            return new Vector2(x, y);
        }
    }
}
#endif