using System;
using System.Collections.Generic;
using UnityEngine;

namespace Road_Generation {
    public class SplineRoad : MonoBehaviour {
        [SerializeField] private float width;
        [SerializeField] private int resolution;

        private SplineSampler _splineSampler;
        private MeshFilter _meshFilter;

        private List<Vector3> _leftVerts;
        private List<Vector3> _rightVerts;

        private void OnEnable() {
            
        }

        private void Awake() {
            _splineSampler = GetComponent<SplineSampler>();
            _meshFilter = GetComponent<MeshFilter>();
            
            GetVerts();
            BuildMesh();
        }

        private void GetVerts() {
            _leftVerts = new List<Vector3>();
            _rightVerts = new List<Vector3>();

            float step = 1f / resolution;
            for (int i = 0; i < resolution + 1; i++) {
                float t = step * i;
                _splineSampler.SampleSplineWidth(t, width, out Vector3 leftVert, out Vector3 rightVert);
                _leftVerts.Add(leftVert);
                _rightVerts.Add(rightVert);
            }
        }

        private void BuildMesh() {
            Mesh mesh = new Mesh();
            List<Vector3> verts = new List<Vector3>();
            List<int> tris = new List<int>();
            int offset;

            // Iterate verts and build a face
            for (int i = 1; i < resolution + 1; i++) {
                Vector3 p1 = _leftVerts[i - 1];
                Vector3 p2 = _rightVerts[i - 1];
                Vector3 p3 = _leftVerts[i];
                Vector3 p4 = _rightVerts[i];

                offset = 4 * (i - 1);

                int t0 = offset + 0;
                int t1 = offset + 1;
                int t2 = offset + 2;
                int t3 = offset + 3;

                verts.AddRange(new List<Vector3> { p1, p2, p3, p4 });
                tris.AddRange(new List<int> { t0, t2, t3, t3, t1, t0 });
            }

            mesh.SetVertices(verts);
            mesh.SetTriangles(tris, 0);
            _meshFilter.mesh = mesh;
        }
    }
}