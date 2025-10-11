using System;
using UnityEngine;
using UnityEngine.Splines;
using AK.Wwise;    // Wwise API namespace

namespace Road_Generation {
    [RequireComponent(typeof(SplineAnimate))]
    public class SplineAnimateFootsteps : MonoBehaviour {
        [SerializeField] private SplineAnimate splineAnimator;
        
        [Header("Footstep Settings")]
        [SerializeField, Tooltip("Wwise Event for footstep")]
        private AK.Wwise.Event footstepEvent;
        [SerializeField, Tooltip("Distance (in world units) between steps")] 
        private float stepDistance = 2f;
        
        [Header("Footstep Emitters")]
        [SerializeField, Tooltip("Empty child positioned at the left foot location")]
        private Transform leftFootEmitter;
        [SerializeField, Tooltip("Empty child positioned at the right foot location")]
        private Transform rightFootEmitter;

        private Vector3 _lastPosition;
        private float _distanceAccum;
        private bool _useLeftNext = true;
        
        private void OnEnable() {
            if (splineAnimator == null) splineAnimator = GetComponent<SplineAnimate>();
            
            splineAnimator.Updated += HandleSplineUpdated;

            // start tracking from wherever we are when enabled
            _lastPosition    = transform.position;
            _distanceAccum   = 0f;
        }

        private void OnDisable() {
            splineAnimator.Updated -= HandleSplineUpdated;
        }

        private void HandleSplineUpdated(Vector3 position, Quaternion rotation) {
            // 1. Move the GameObject
            // transform.SetPositionAndRotation(position, rotation);

            // 2. Accumulate how far we've moved since last step
            _distanceAccum += Vector3.Distance(_lastPosition, position);
            _lastPosition = position;

            // 3. If we've gone far enough, play footstep and reset
            if (_distanceAccum < stepDistance) return;
            
            var emitterGO = _useLeftNext ? leftFootEmitter.gameObject : rightFootEmitter.gameObject;
            var footString = _useLeftNext ? "LEFT" : "RIGHT";
            
            footstepEvent.Post(emitterGO);
            LSLController.Instance.LogFootsteps($"Footstep played: {footString}");

            // alternate foot and subtract distance
            _useLeftNext = !_useLeftNext;
            
            _distanceAccum -= stepDistance;
        }
    }
}