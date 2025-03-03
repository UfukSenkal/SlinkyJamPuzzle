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
        public AnimationCurve movementCurve;
        public float wobbleAmount = 0.2f;
        public float wobbleSpeed = 5f;
        public float segmentFollowSpeed = 5f;
        public float scaleEffectDuration = 0.2f;

        private List<Transform> _segments;
        private Vector3 _targetPosition;
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
            _targetPosition = startPos;
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
                _isSelected = true;
                GridManager gridManager = _levelPlayer.Get<GridManager>();
                if (gridManager != null)
                {
                    gridManager.PlaceSlinkyInLowerGrid(this);
                }
            }
        }

        public void MoveToTarget(Vector3 targetPos)
        {
            _targetPosition = targetPos;
            MoveSlinky();
        }
        private void MoveSlinky()
        {
            _isMoving = true;
            Transform firstSegment = _segments[0];
            Transform lastSegment = _segments[_segments.Count - 1];

            firstSegment.DOMoveY(firstSegment.position.y + 1f, 0.3f).OnComplete(() =>
            {
                firstSegment.DOMove(_targetPosition, 0.5f).OnUpdate(UpdateBridgeShape).OnComplete(() =>
                {
                    AlignSegments();
                    _isMoving = false;
                    _isSelected = false;
                });
            });
        }

        private void UpdateBridgeShape()
        {
            for (int i = 1; i < _segments.Count - 1; i++)
            {
                float t = (float)i / (_segments.Count - 1);
                Vector3 bridgePos = Vector3.Lerp(_segments[0].position, _segments[_segments.Count - 1].position, t);
                float arcHeight = Mathf.Sin(t * Mathf.PI) * 0.5f;
                _segments[i].position = bridgePos + new Vector3(0, arcHeight, 0);
            }
        }

        private void AlignSegments()
        {
            for (int i = 0; i < _segments.Count; i++)
            {
                var segment = _segments[i];
                if (i >= _segments.Count - 1)
                    segment.DOMove(_targetPosition, 0.2f).OnComplete(() => onMovementComplete?.Invoke());
                else
                    segment.DOMove(_targetPosition, 0.2f);
            }
        }
    }
}