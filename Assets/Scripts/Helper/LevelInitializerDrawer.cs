using HybridPuzzle.SlinkyJam.Helper;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace HybridPuzzle.SlinkyJam.Helper
{
    public class LevelInitializerAttribute : PropertyAttribute { }

    [CustomPropertyDrawer(typeof(LevelInitializerAttribute))]
    public class LevelInitializerDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            Object selectedObj = EditorGUI.ObjectField(position, label, property.objectReferenceValue, typeof(Component), true);

            if (selectedObj is Component component)
            {
                if (component is ILevelInitializer)
                {
                    property.objectReferenceValue = component;
                }
                else
                {
                    GameObject go = component.gameObject;
                    var validComponent = go.GetComponents<Component>().FirstOrDefault(c => c is ILevelInitializer);
                    property.objectReferenceValue = validComponent;
                }
            }
            else
            {
                property.objectReferenceValue = null;
            }

            EditorGUI.EndProperty();
        }
    }
}