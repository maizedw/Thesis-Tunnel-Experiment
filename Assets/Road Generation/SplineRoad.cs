using System;
using System.Collections.Generic;
using System.Linq;
using Unity.XR.CoreUtils;
using UnityEditor.Splines;
using UnityEngine;
using UnityEngine.Splines;

namespace Road_Generation {
    [ExecuteInEditMode]
    public class SplineRoad : MonoBehaviour {
        [SerializeField] private float width;
        [SerializeField] private int resolution;
        
        private SplineSampler _splineSampler;
        private Spline _targetSpline;            // cache the spline we care about
        private MeshFilter _meshFilter;

        private List<Vector3> _leftVerts;
        private List<Vector3> _rightVerts;

        private void OnEnable() {
            _splineSampler = GetComponent<SplineSampler>();
            _targetSpline = _splineSampler.splineContainer.Spline;      // or .Container.Spline if that’s your API
            _meshFilter = GetComponent<MeshFilter>();
            
#if UNITY_EDITOR
            EditorSplineUtility.AfterSplineWasModified += OnSplineModified;
#endif
        }
        
        private void OnDisable()
        {
#if UNITY_EDITOR
            EditorSplineUtility.AfterSplineWasModified -= OnSplineModified;
#endif
        }

        private void OnSplineModified(Spline spline) {
            // only rebuild if *this* spline was the one edited
            if (spline == _targetSpline)
                Rebuild();
        }
        
        private void Rebuild() {
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
            List<Vector2> uvs = new List<Vector2>();
            int offset;
            Vector3 pos = transform.position;

            // Iterate verts and build a face
            for (int i = 1; i < resolution + 1; i++) {
                Vector3 p1 = _leftVerts[i - 1] - pos;
                Vector3 p2 = _rightVerts[i - 1] - pos;
                Vector3 p3 = _leftVerts[i] - pos;
                Vector3 p4 = _rightVerts[i] - pos;

                offset = 4 * (i - 1);

                int t0 = offset + 0;
                int t1 = offset + 1;
                int t2 = offset + 2;
                int t3 = offset + 3;

                verts.AddRange(new List<Vector3> { p1, p2, p3, p4 });
                tris.AddRange(new List<int> { t0, t2, t3, t3, t1, t0 });
                uvs.AddRange(new List<Vector2> { new Vector2(p1.x, p1.z), new Vector2(p2.x, p2.z), new Vector2(p3.x, p3.z), new Vector2(p4.x, p4.z) });
            }

            mesh.SetVertices(verts);
            mesh.SetTriangles(tris, 0);
            mesh.SetUVs(0, uvs);
            mesh.SetTangents(Enumerable.Repeat(Vector4.one, verts.Count).ToList());
            _meshFilter.mesh = mesh;
        }
    }
}