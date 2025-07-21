using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerArea : MonoBehaviour {

	public GameObject targetArea;
	public bool startAsVisible = false;

	void Start() {
		if (startAsVisible) {			
			targetArea.SetActive (true);
		}

		if (!startAsVisible) {
			targetArea.SetActive (false);
		}
		
	}


	void OnTriggerEnter(Collider other){
		targetArea.SetActive (true);
		
	}

	void OnTriggerExit(Collider other){
		targetArea.SetActive (false);
	}


}
