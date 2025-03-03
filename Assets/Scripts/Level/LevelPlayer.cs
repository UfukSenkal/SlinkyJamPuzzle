using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HybridPuzzle.SlinkyJam.Level
{
    public class LevelPlayer : MonoBehaviour
    {
        private Dictionary<Type, Component> _components = new Dictionary<Type, Component>();

        public void Register<T>(T component) where T : Component
        {
            Type type = component.GetType();
            if (!_components.ContainsKey(type))
            {
                _components[type] = component;
            }
        }

        public T Get<T>() where T : Component
        {
            Type type = typeof(T);
            return _components.TryGetValue(type, out Component component) ? (T)component : null;
        }
    }
}
