using System;
using UnityEngine;
using UnityEngine.Splines;

public class EndCanvasController : MonoBehaviour {
    [SerializeField] private SplineAnimate splineAnimator;
    private Canvas _canvas;

    private void OnEnable() {
        if (splineAnimator == null) splineAnimator = GetComponent<SplineAnimate>();
        if (_canvas == null) _canvas = GetComponent<Canvas>();
        
        splineAnimator.Completed += HandleSplineCompleted;
    }
    
    private void OnDisable() {
        splineAnimator.Completed -= HandleSplineCompleted;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        _canvas.enabled = false;
    }

    // Update is called once per frame
    void Update() { }

    private void HandleSplineCompleted() {
        _canvas.enabled = true;
        LSLController.Instance.LogControl("Walk ended");
    }

    public void DirectionButtonClicked(string direction) {
        LSLController.Instance.LogControl($"Direction: {direction}");
    }
}