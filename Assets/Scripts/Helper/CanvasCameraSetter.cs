using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HybridPuzzle.SlinkyJam.UI
{
    [RequireComponent(typeof(Canvas))]
    public class CanvasCameraSetter : MonoBehaviour
    {
        
        private void Awake()
        {
            var canvas = GetComponent<Canvas>();
            canvas.worldCamera = Camera.main;
        }
    }
}
