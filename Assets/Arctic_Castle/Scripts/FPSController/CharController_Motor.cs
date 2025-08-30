using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharController_Motor : MonoBehaviour, ISaveable {

	public float speed = 2f;
	public float sensitivity = 10.0f;
	CharacterController character;
	public GameObject cam;
	float moveFB, moveLR;
	float rotX, rotY;
	public bool webGLRightClickRotation = true;



	void Start(){
		//LockCursor ();
		character = GetComponent<CharacterController> ();
		if (Application.isEditor) {
			webGLRightClickRotation = false;
			sensitivity = sensitivity * 2;
		}
	}



	void Update(){
		moveFB = Input.GetAxis ("Horizontal") * speed;
		moveLR = Input.GetAxis ("Vertical") * speed;

		rotX = Input.GetAxis ("Mouse X") * sensitivity;
		rotY = Input.GetAxis ("Mouse Y") * sensitivity;

		Vector3 movement = new Vector3 (moveFB, -9.8f, moveLR);



		if (webGLRightClickRotation) {
			if (Input.GetKey (KeyCode.Mouse0)) {
				CameraRotation (cam, rotX, rotY);
			}
		} else if (!webGLRightClickRotation) {
			CameraRotation (cam, rotX, rotY);
		}

		movement = transform.rotation * movement;
		character.Move (movement * Time.deltaTime);
	}


	void CameraRotation(GameObject cam, float rotX, float rotY){		
		transform.Rotate (0, rotX * Time.deltaTime, 0);
		cam.transform.Rotate (-rotY * Time.deltaTime, 0, 0);
	}

    public object CaptureState()
    {
		return new Vector3Wrapper (transform.position);
			}

    public void RestoreState(object state)
    {
		transform.position = (state as Vector3Wrapper).Get();
	}
}
public class Vector3Wrapper
{
	public float x, y, z;
	public Vector3Wrapper(Vector3 pos)
	{
		x = pos.x;
		y = pos.y;
		z = pos.z;

	}

	public Vector3 Get()
	{
		return new Vector3 (x, y, z);
	}
}