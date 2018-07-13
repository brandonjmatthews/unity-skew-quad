using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkewDemo : MonoBehaviour {
	public SkewQuad quad;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		quad.UpdateMeshAndTexture();
	}
}
