using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeosphereGen : MonoBehaviour
{
	public float Radius = 0.5f;

	public class Tri
	{
		public readonly Vector3[] Verts = new Vector3[3];

		public Vector3 a
		{
			get
			{
				return Verts[0];
			}
			set
			{
				Verts[0] = value;
			}
		}

		public Vector3 b
		{
			get
			{
				return Verts[1];
			}
			set
			{
				Verts[1] = value;
			}
		}

		public Vector3 c
		{
			get
			{
				return Verts[2];
			}
			set
			{
				Verts[2] = value;
			}
		}

		public List<Tri> Subdivide()
		{
			var subTris = new List<Tri>();

			var ab = b - a;
			var ac = c - a;
			var bc = c - b;
			var d = a + ab * 0.5f;
			var e = b + bc * 0.5f;
			var f = a + ac * 0.5f;

			var tri0 = new Tri();
			tri0.a = a.normalized;
			tri0.b = d.normalized;
			tri0.c = f.normalized;

			var tri1 = new Tri();
			tri1.a = d.normalized;
			tri1.b = b.normalized;
			tri1.c = e.normalized;

			var tri2 = new Tri();
			tri2.a = f.normalized;
			tri2.b = e.normalized;
			tri2.c = c.normalized;

			var tri3 = new Tri();
			tri3.a = f.normalized;
			tri3.b = d.normalized;
			tri3.c = e.normalized;

			subTris.Add(tri0);
			subTris.Add(tri1);
			subTris.Add(tri2);
			subTris.Add(tri3);

			return subTris;
		}
	}

	public void Awake()
	{
		var startingTris = new List<Tri>();
		var xz = new Vector3(-1, 0, 1).normalized;

		for (int i = 0; i < 8; i++)

		{
			var newTri = new Tri();
			var rot0 = i / 4.0f * 360.0f;
			var rot1 = (i + 1) / 4.0f * 360.0f;

			newTri.a = (i < 4 ? Vector3.up : Vector3.down);
			newTri.b = Quaternion.Euler(Vector3.up * (i < 4 ? rot0 : rot1)) * xz;
			newTri.c = Quaternion.Euler(Vector3.up * (i < 4 ? rot1 : rot0)) * xz;

			startingTris.Add(newTri);
		}

		var srcTris = startingTris;
		List<Tri> subDivided;

		for (int i = 0; i < 5; i++)
		{
			subDivided = new List<Tri>();

			foreach (var tri in srcTris)
			{
				subDivided.AddRange(tri.Subdivide());
			}

			srcTris = subDivided;
		}

		GetComponent<MeshFilter>().sharedMesh = TrisToMesh(srcTris);
	}

	public Mesh TrisToMesh(List<Tri> tris)
	{
		var vertices = new List<Vector3>();
		var triangles = new List<int>();

		foreach (var tri in tris)
		{
			vertices.Add(tri.a);
			vertices.Add(tri.b);
			vertices.Add(tri.c);
			triangles.Add(triangles.Count);
			triangles.Add(triangles.Count);
			triangles.Add(triangles.Count);
		}

		var mesh = new Mesh();

		mesh.SetVertices(vertices);
		mesh.SetTriangles(triangles, 0);

		return mesh;
	}
}
