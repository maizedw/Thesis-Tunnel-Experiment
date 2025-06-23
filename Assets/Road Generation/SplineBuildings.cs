using System;
using System.Collections.Generic;
using UnityEngine;

namespace Road_Generation {
    [ExecuteInEditMode]
    public class SplineBuildings : MonoBehaviour {
        [SerializeField] private float width;
        [SerializeField] private int resolution;
        
        [SerializeField] private GameObject building;
        [SerializeField] private GameObject buildingsHolder;
        
        private SplineSampler _splineSampler;
        
        private List<Vector3> _leftVerts;
        private List<Vector3> _rightVerts;
        private List<Quaternion> _tangents;

        private void Awake() {
            _splineSampler = GetComponent<SplineSampler>();
            GetVerts();
            BuildBuildings();
        }

        private void GetVerts() {
            _leftVerts = new List<Vector3>();
            _rightVerts = new List<Vector3>();
            _tangents = new List<Quaternion>();

            float step = 1f / resolution;
            for (int i = 0; i < resolution + 1; i++) {
                float t = step * i;
                _splineSampler.SampleSplineWidth(t, width, out Vector3 leftVert, out Vector3 rightVert);
                _leftVerts.Add(leftVert);
                _rightVerts.Add(rightVert);

                var tangent = _splineSampler.SampleSplineTangent(t);
                _tangents.Add(Quaternion.LookRotation(tangent, Vector3.up));
            }
        }
        
        private void BuildBuildings() {
            for(var i = buildingsHolder.transform.childCount - 1; i >= 0; i--) {
                DestroyImmediate(buildingsHolder.transform.GetChild(i).gameObject);
            }
            
            for (int i = 0; i < _leftVerts.Count; i++) {
                Vector3 leftPoint = _leftVerts[i];
                Vector3 rightPoint = _rightVerts[i];
                Quaternion tangent = _tangents[i];

                var leftBuilding = Instantiate(building, leftPoint, tangent);
                var rightBuilding = Instantiate(building, rightPoint, tangent);
                
                leftBuilding.transform.SetParent(buildingsHolder.transform);
                rightBuilding.transform.SetParent(buildingsHolder.transform);
                i += Mathf.FloorToInt(building.transform.localScale.x);
            }
        }
    }
}