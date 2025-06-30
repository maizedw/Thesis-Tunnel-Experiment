using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.Splines;

namespace Road_Generation {
    [ExecuteInEditMode]
    public class SplineSampler : MonoBehaviour {
        [SerializeField] public SplineContainer splineContainer;
        [SerializeField] private int splineIndex;

        private float3 _position;
        private float3 _tangent;
        private float3 _upVector;

        public void SampleSplineWidth(float t, float width, out Vector3 p1, out Vector3 p2) {
            splineContainer.Evaluate(splineIndex, t, out _position, out _tangent, out _upVector);
            // Tangent is the (forward) direction of travel along the spline to the next point;
            // Find the *right* direction based on this.
            float3 right = Vector3.Cross(_tangent, _upVector).normalized;
            p1 = _position + (right * width);
            p2 = _position + (-right * width);
        }

        public Vector3 SampleSpline(float t) {
            splineContainer.Evaluate(splineIndex, t, out _position, out _tangent, out _upVector);

            return _position;
        }
        
        public Vector3 SampleSplineTangent(float t) {
            splineContainer.Evaluate(splineIndex, t, out _position, out _tangent, out _upVector);
            // Tangent is the (forward) direction of travel along the spline to the next point;
            // Find the *right* direction based on this.
            return Vector3.Cross(_tangent, _upVector).normalized;
        }
    }
}