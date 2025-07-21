using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCell : MonoBehaviour {

	public GameObject objectsInsideCell;
	public GameObject objectsOutsideCell;
	public bool playerStartsInsideCell = false;

	void Start() {
		if (playerStartsInsideCell) {			
			objectsInsideCell.SetActive (true);
			objectsOutsideCell.SetActive (false);
		}

		if (!playerStartsInsideCell) {
			objectsInsideCell.SetActive (false);
			objectsOutsideCell.SetActive (true);
		}
		
	}


	void OnTriggerEnter(Collider other){
		objectsInsideCell.SetActive (true);
		objectsOutsideCell.SetActive (false);
		
	}

	void OnTriggerExit(Collider other){
		objectsInsideCell.SetActive (false);
		objectsOutsideCell.SetActive (true);
	}


}
