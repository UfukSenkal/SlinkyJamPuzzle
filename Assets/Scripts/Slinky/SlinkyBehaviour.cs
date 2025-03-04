using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using HybridPuzzle.SlinkyJam.Grid;

namespace HybridPuzzle.SlinkyJam.Slinky
{
    public class SlinkyBehaviour : MonoBehaviour
    {
        public GameObject segmentPrefab;
        public float segmentSpacing = 0.5f;
        public float segmentFollowSpeed = 5f;
        [SerializeField] private float _travelTime = 0.5f; // Hareket süresi
        [SerializeField] private float _delayBetweenSegments = 0.05f; // Segmentler arasý gecikme
        [SerializeField] private float _springFactor = 0.15f; // Yay esnekliði
        [SerializeField] private Ease _movementEase = Ease.InOutSine;

        private List<Transform> _segments;
        private bool _isMoving = false;
        private bool _isSelected = false;
        private Level.LevelPlayer _levelPlayer;

        public Action onMovementComplete;

        public int SlotIndex { get; set; }
        public Color Color { get; private set; }
        public void Initialize(Vector3 startPos, Vector3 endPos, Color color)
        {
            _levelPlayer = transform.root.GetComponent<Level.LevelPlayer>();
            Color = color;
            transform.position = startPos;
            _segments = new List<Transform>();
            _isSelected = false;
            float distance = Vector3.Distance(startPos, endPos);
            int segmentCount = Mathf.CeilToInt(distance / segmentSpacing);


            for (int i = 0; i < segmentCount; i++)
            {
                Vector3 segmentPosition = CalculateSegmentPosition(startPos, endPos, i, segmentCount);
                Quaternion segmentRotation = CalculateSegmentRotation(startPos, endPos, i, segmentCount);

                GameObject segment = Instantiate(segmentPrefab, segmentPosition, segmentRotation, transform);
                segment.GetComponent<Renderer>().material.color = color; 
                _segments.Add(segment.transform);
            }
        }
        private Vector3 CalculateSegmentPosition(Vector3 startPos, Vector3 endPos, int index, int totalSegments)
        {
            float t = (float)index / (totalSegments - 1); 
            Vector3 linearPosition = Vector3.Lerp(startPos, endPos, t); 

            float parabola = Mathf.Sin(t * Mathf.PI); 
            Vector3 offset = new Vector3(0, parabola , 0);

            return linearPosition + offset;
        }

        private Quaternion CalculateSegmentRotation(Vector3 startPos, Vector3 endPos, int index, int totalSegments)
        {
            if (index == 0 || index == totalSegments - 1)
            {
                return Quaternion.identity; 
            }

            Vector3 direction = (endPos - startPos).normalized;
            return Quaternion.LookRotation(direction);
        }

        public void OnSegmentClicked()
        {
            if (!_isSelected && !_isMoving)
            {
                GridManager gridManager = _levelPlayer.Get<GridManager>();
                if (gridManager != null)
                {

                    _isSelected = gridManager.TryPlaceSlinkyInLowerGrid(this);
                }
            }
        }

        public void MoveToTarget(Vector3 targetPosition)
        {
            if (_isMoving) return;
            _isMoving = true;

            Vector3 startPos = _segments[0].position;
            Vector3 midPoint = (startPos + targetPosition) / 2 + Vector3.up * (_springFactor * Vector3.Distance(startPos, targetPosition));


            for (int i = 0; i < _segments.Count; i++)
            {
                int index = i;
                float delay = i * _delayBetweenSegments;

                _segments[index].DOPath(new Vector3[] { midPoint, targetPosition }, _travelTime, PathType.CatmullRom)
                    .SetDelay(delay)
                    .SetEase(_movementEase);
            }

            DOVirtual.DelayedCall(_travelTime + _delayBetweenSegments * _segments.Count, () =>
            {
                _isMoving = false;
                onMovementComplete?.Invoke();
            });
        }

    }
}