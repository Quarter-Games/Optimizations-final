using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CullDistances : MonoBehaviour {

	public float[] distances = new float[32];

	void Start () {
		Camera cam = GetComponent<Camera> ();
		cam.layerCullDistances = distances;
		
	}

}
