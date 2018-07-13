using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkewQuad : MonoBehaviour {
	public bool enableSkew = true;
	public Vector3 TopLeft;
	public Vector3 TopRight;
	public Vector3 BottomLeft;
	public Vector3 BottomRight;

	Mesh mesh;
	MeshFilter mf;

	void Start () {
		mesh = new Mesh();
		mf = GetComponent<MeshFilter>();
	}
	
	public void UpdateMeshAndTexture() {
		bool updated = false;
		// Build the Quad
		Vector3[] vertices = new Vector3[4] {
			BottomLeft,
			BottomRight,
			TopRight,
			TopLeft
		};

		mesh.vertices = vertices;

		int[] triangles = new int[6] {
			0,2,1, 
			0,3,2
		};

		mesh.triangles = triangles;

		Vector3[] normals = new Vector3[4] {
			-Vector3.forward,
			-Vector3.forward,
			-Vector3.forward,
			-Vector3.forward
		};
		mesh.normals = normals;

		Vector2[] uv = new Vector2[4] {
			new Vector2(0,0),
			new Vector2(1,0),
			new Vector2(1,1),
			new Vector2(0,1),
		};

		// I used uv2 (qu, qv) and uv3 (q), there is potential for this to be an issue on different devices. 

		float ax = vertices[2].x - vertices[0].x;
		float ay = vertices[2].y - vertices[0].y;
		float bx = vertices[3].x - vertices[1].x;
		float by = vertices[3].y - vertices[1].y;

		float cross = (ax * by) - (ay * bx);
		
		Vector2[] uv2 = uv;

		Vector2[] uv3 = new Vector2[4] {
			new Vector2(1,0),
			new Vector2(1,0),
			new Vector2(1,0),
			new Vector2(1,0),
		};

		// Only skew if it's possible (ie. clockwise tri vertices and convex shape)
		if (cross != 0 && enableSkew) {
			float cy = vertices[0].y - vertices[1].y;
			float cx = vertices[0].x - vertices[1].x;

			float s = (ax * cy - ay * cx) / cross;
	
			if (s > 0 && s < 1) {
				float t = (bx * cy - by * cx) / cross;

				if (t > 0 && t < 1) {
					float q0 = 1 / (1-t);
					float q1 = 1 / (1-s);
					float q2 = 1 / t;
					float q3 = 1 / s;
					uv2[0] = new Vector2(uv[0].x * q0, uv[0].y * q0);
					uv2[1] = new Vector2(uv[1].x * q1, uv[1].y * q1);
					uv2[2] = new Vector2(uv[2].x * q2, uv[2].y * q2);
					uv2[3] = new Vector2(uv[3].x * q3, uv[3].y * q3);
					uv3[0] = new Vector2(q0, 0);
					uv3[1] = new Vector2(q1, 0);
					uv3[2] = new Vector2(q2, 0);
					uv3[3] = new Vector2(q3, 0);
					updated = true;
				}
			}
		}

		mesh.uv = uv;
		mesh.uv2 = uv2;
		mesh.uv3 = uv3;
		if (updated || !enableSkew) {
			mf.mesh = mesh;
		} else {
			throw new System.Exception("Shape must be convex and triangle vertices must be clockwise.");
		}
		
	}
}
