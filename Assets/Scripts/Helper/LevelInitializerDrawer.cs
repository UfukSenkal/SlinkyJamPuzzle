using HybridPuzzle.SlinkyJam.Helper;
using UnityEditor;
using UnityEngine;
using HybridPuzzle.SlinkyJam.Level;
using System.Collections.Generic;
using System.Linq;

namespace HybridPuzzle.SlinkyJam.Helper
{
    [CustomPropertyDrawer(typeof(Component), true)] 
    public class LevelInitializerDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            Object currentObj = property.objectReferenceValue;

            position = EditorGUI.PrefixLabel(position, label);
            Object selectedObj = EditorGUI.ObjectField(position, currentObj, typeof(GameObject), true);

            if (selectedObj is GameObject go)
            {
                List<Component> validComponents = go.GetComponents<Component>()
                    .Where(c => c is ILevelInitializer) 
                    .ToList();

                if (validComponents.Count > 0)
                {
                    string[] options = validComponents.Select(c => c.GetType().Name).ToArray();
                    int selectedIndex = currentObj != null ? validComponents.IndexOf(currentObj as Component) : 0;
                    selectedIndex = EditorGUI.Popup(new Rect(position.x, position.y + 20, position.width, position.height), "Select Component", selectedIndex, options);

                    property.objectReferenceValue = validComponents[selectedIndex];
                }
                else
                {
                    property.objectReferenceValue = null;
                }
            }

            EditorGUI.EndProperty();
        }
    }
}