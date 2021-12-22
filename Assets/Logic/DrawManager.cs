using Assets.Logic.ColorChanger;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Assets.Logic
{
    public class DrawManager : MonoBehaviour
    {
        [SerializeField] private ColorManager _colorManager;

        [Header("Line Renderer configuration")]
        [SerializeField] private float _lineWidth = 0.05f;
        [SerializeField] private Color _color = Color.white;
        [SerializeField] private float _minDistanceTreshold = 0.01f;
        [SerializeField] Material _lineMaterial;

        [SerializeField] private GameObject MainCanvas;

        [Header("HandMenu")]
        [SerializeField] private GameObject MainHandMenu;
        [SerializeField] private GameObject DrawHandMenu;
        [SerializeField] private GameObject PauseButton;
        [SerializeField] private GameObject UnpauseButton;


        public Handedness myHandedness;
        private MixedRealityPose jointPose;

        private GameObject _parentContainer;

        private bool _canDraw = false;
        private bool _drawPaused = false;
        private bool _drawPausedExplicitly = false;

        private Vector3 _prevPointDistance = Vector3.zero;
        private LineRenderer _currentLineRenderer;
        private LineRenderer _prevLineRenderer;
        private List<LineRenderer> _lines = new List<LineRenderer>();
        private int _positionCount = 0;

        private void Update()
        {
            if (_canDraw && !_drawPaused && !_drawPausedExplicitly)
            {
                if (HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, myHandedness, out jointPose))
                {
                    if (_parentContainer == null)
                    {
                        _parentContainer = new GameObject("Canvas");
                        _parentContainer.transform.SetParent(MainCanvas.transform);
                    }

                    if (_prevLineRenderer == null)
                    {
                        AddNewLineRenderer(jointPose.Position);
                    }
                    else
                    {
                        UpdateLine(jointPose.Position);
                    }
                }
                else
                {
                    ResetLineRendererStates();
                }

            }
        }

        private void ResetLineRendererStates()
        {
            _prevLineRenderer = null;
            _prevPointDistance = Vector3.zero;
            _positionCount = 0;
        }

        public void PauseDraw()
        {
            _drawPaused = true;

            ResetLineRendererStates();
        }

        public void UnpauseDraw()
        {
            _drawPaused = false;

            DrawHandMenu.GetComponentInChildren<GridObjectCollection>().UpdateCollection();
        }

        public void PauseDrawExplicitly()
        {
            _drawPausedExplicitly = true;

            UnpauseButton.SetActive(true);
            PauseButton.SetActive(false);

            DrawHandMenu.GetComponentInChildren<GridObjectCollection>().UpdateCollection();

            ResetLineRendererStates();
        }

        public void UnpauseDrawExplicitly()
        {
            _drawPausedExplicitly = false;

            UnpauseButton.SetActive(false);
            PauseButton.SetActive(true);

            DrawHandMenu.GetComponentInChildren<GridObjectCollection>().UpdateCollection();
        }


        public bool GetCanDraw()
        {
            return _canDraw;
        }

        public void StartDraw()
        {
            DrawHandMenu.SetActive(true);
            MainHandMenu.SetActive(false);

            _canDraw = true;
        }

        public void EndDraw()
        {
            ResetLineRendererStates();
            _parentContainer = null;
            _canDraw = false;

            DrawHandMenu.SetActive(false);
            MainHandMenu.SetActive(true);
        }

        //public void SaveDrawing(string drawingName = "temp")
        //{
        //    if (_parentContainer != null)
        //    {
        //        _parentContainer.name = drawingName;
        //    }
        //}

        //public void LoadDrawing(string drawingName)
        //{
        //    //Instantiate(PrefabUtility.LoadPrefabContents(drawingName));
        //}

        public void ClearCanvas()
        {
            foreach (Transform child in MainCanvas.transform)
            {
                Destroy(child.gameObject);
            }
        }

        public void RestartCurrent()
        {
            foreach (Transform lines in _parentContainer.transform)
            {
                Destroy(lines.gameObject);
            }
        }

        private void AddNewLineRenderer(Vector3 penPosition)
        {
            GameObject go = new GameObject($"LineRenderer{_lines.Count + 1}");

            go.transform.parent = _parentContainer.transform;

            int positionCount = 2;
            go.transform.position = penPosition;

            LineRenderer lineRenderer = go.AddComponent<LineRenderer>();

            lineRenderer.startWidth = _lineWidth;
            lineRenderer.endWidth = _lineWidth;
            lineRenderer.useWorldSpace = true;
            lineRenderer.positionCount = positionCount;
            lineRenderer.numCapVertices = 90;
            lineRenderer.startColor = _colorManager.GetColor();
            lineRenderer.endColor = _colorManager.GetColor();
            lineRenderer.material.color = _colorManager.GetColor();

            _currentLineRenderer = lineRenderer;
            _prevLineRenderer = _currentLineRenderer;

            _lines.Add(lineRenderer);
        }

        private void UpdateLine(Vector3 penPosition)
        {
            if (_prevPointDistance == Vector3.zero)
            {
                _prevPointDistance = penPosition;
            }

            if (Mathf.Abs(Vector3.Distance(_prevPointDistance, penPosition)) >= _minDistanceTreshold)
            {
                _prevPointDistance = penPosition;
                AddPoint(_prevPointDistance);
            }
        }

        private void AddPoint(Vector3 position)
        {
            _positionCount++;
            _currentLineRenderer.positionCount = _positionCount;

            _currentLineRenderer.SetPosition(_positionCount - 1, position);
       }
    }
}
