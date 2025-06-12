using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.Splines;

namespace Road_Generation {
    [ExecuteInEditMode]
    public class SplineSampler : MonoBehaviour {
        [SerializeField] private SplineContainer splineContainer;
        [SerializeField] private int splineIndex;
        [SerializeField] [Range(0f, 1f)] private float time;

        private float3 _position;
        private float3 _tangent;
        private float3 _upVector;

        private void Update() {
            splineContainer.Evaluate(splineIndex, time, out _position, out _tangent, out _upVector);
        }

        private void OnDrawGizmos() {
            Handles.matrix = transform.localToWorldMatrix;
            Handles.SphereHandleCap(0, _position, Quaternion.identity, 1f, EventType.Repaint);
        }

        public void SampleSplineWidth(float t, float width, out Vector3 p1, out Vector3 p2) {
            splineContainer.Evaluate(splineIndex, t, out _position, out _tangent, out _upVector);
            // Tangent is the (forward) direction of travel along the spline to the next point;
            // Find the *right* direction based on this.
            float3 right = Vector3.Cross(_tangent, _upVector).normalized;
            p1 = _position + (right * width);
            p2 = _position + (-right * width);
        }
        
        public Vector3 SampleSplineTangent(float t) {
            splineContainer.Evaluate(splineIndex, t, out _position, out _tangent, out _upVector);
            // Tangent is the (forward) direction of travel along the spline to the next point;
            // Find the *right* direction based on this.
            return Vector3.Cross(_tangent, _upVector).normalized;
        }
    }
}