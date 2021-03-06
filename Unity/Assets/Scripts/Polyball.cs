﻿using UnityEngine;
using System.Collections;

public class Polyball : MonoBehaviour {

	private Vector3[] vertsPos;
	private Vector2[] vertsUV;
	private int[] vertsIndices;
	private Color32[] vertsColor;

	Vector3 GetRandomSpherePos()
	{
		int numTheta = 6;
		int numPhi = 5;

		float deltaTheta = 2.0f * Mathf.PI / numTheta;
		float deltaPhi = Mathf.PI / numPhi;
		float theta = Random.Range (0, numTheta) * deltaTheta;
		float phi = Random.Range (0, numPhi) * deltaPhi - Mathf.PI*0.5f;

		float radius = 1.0f;
		float x = radius * Mathf.Cos( phi ) * Mathf.Sin ( theta );
		float y = radius * Mathf.Sin( phi ) * Mathf.Sin ( theta );
		float z = radius * Mathf.Cos ( theta );

		return new Vector3 (x, y, z);
	}

	Color32 GetFaceColor(int aFace) {
		//Color32 color = new Color32( (byte)(Random.Range(0,255)), (byte)(Random.Range(0,255)), (byte)(Random.Range(0,255)), 255 );


		HSBColor baseColor = new HSBColor (new Color (243.0f/255.0f, 225/255.0f, 93.0f/255.0f));

		int numSteps = 4;
		baseColor.h += Random.Range (0, numSteps) / (float)(numSteps) + Random.Range (-0.05f,0.05f);
		baseColor.s = Random.Range (0.75f, 0.9f);
		baseColor.b = Random.Range (0.8f, 1.0f);

		Color32 finalColor = baseColor.ToColor();
		return finalColor;
	}

	void CreateMesh() {
		Mesh mesh = new Mesh ();
		GetComponent<MeshFilter> ().mesh = mesh;

		const int numFaces = 16;
		const int numVertsPerFace = 3;
		const int numVerts = numFaces * numVertsPerFace;

		vertsPos = new Vector3[numVerts];
		vertsUV = new Vector2[numVerts];
		vertsIndices = new int[numVerts];
		vertsColor = new Color32[numVerts];

		for (int i = 0; i < numFaces; i++) {

			int indexOffset = i * numVertsPerFace;

			Color32 color = GetFaceColor(i);

			Vector3 pos0 = GetRandomSpherePos();
			Vector3 pos1 = GetRandomSpherePos();
			Vector3 pos2 = -(pos0+pos1)*0.5f; pos2.Normalize(); pos2 *= pos1.magnitude;
			//pos2 = GetRandomSpherePos();

			vertsPos[0+indexOffset] = pos0;
			vertsPos[1+indexOffset] = pos1;
			vertsPos[2+indexOffset] = pos2;

			vertsColor[0+indexOffset] = color;
			vertsColor[1+indexOffset] = color;
			vertsColor[2+indexOffset] = color;

			float randNum0 = Random.Range(0.0f,1.0f);
			float randNum1 = Random.Range(0.0f,1.0f);
			vertsUV[0+indexOffset] = new Vector2( randNum0, randNum1 );
			vertsUV[1+indexOffset] = new Vector2( randNum0, randNum1 );
			vertsUV[2+indexOffset] = new Vector2( randNum0, randNum1 );

			vertsIndices[0+indexOffset] = indexOffset+0;
			vertsIndices[1+indexOffset] = indexOffset+1;
			vertsIndices[2+indexOffset] = indexOffset+2;
		}

		mesh.vertices = vertsPos;
		mesh.uv = vertsUV;
		mesh.triangles = vertsIndices;
		mesh.colors32 = vertsColor;

		mesh.RecalculateBounds ();
		mesh.RecalculateNormals ();	
	}

	void Update() {
		transform.localScale = Vector3.Lerp (transform.localScale, Vector3.one, 0.1f);
	}

	public void OnClick() {

		transform.localScale *= 1.5f;
	}

	// Use this for initialization
	void Start () {
		CreateMesh ();
	}

}
