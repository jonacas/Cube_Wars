using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementBehaviour : MonoBehaviour {


	public Camera cameraReference;

	Transform initialCameraLocation;
	float minFieldOfView = 30f;
	float maxFieldOfView;

	public float angleForce = 0.0f;

	// Use this for initialization
	void Start () 
	{
		print (cameraReference.name);

		initialCameraLocation = cameraReference.transform;
		maxFieldOfView = cameraReference.fieldOfView + 30;
		
	}

	void ZoomCameraControl()
	{
		float mouseWheel = Input.GetAxis ("Mouse ScrollWheel");
		if (mouseWheel != 0f) 
		{
			if (mouseWheel > 0f) 
			{
				if (cameraReference.fieldOfView > minFieldOfView)
				{
					cameraReference.fieldOfView = cameraReference.fieldOfView - 2;
				}
			}
			else if (mouseWheel < 0f) 
			{
				if (cameraReference.fieldOfView < maxFieldOfView)
				{
					cameraReference.fieldOfView = cameraReference.fieldOfView + 2;
				}
			}
		}
	}

	void PositionCameraControl()
	{
		if (Input.mousePosition.x > (Screen.width / 10) * 9) 
		{
			cameraReference.transform.position = new Vector3 (cameraReference.transform.position.x + Time.deltaTime * 20, 
				cameraReference.transform.position.y , cameraReference.transform.position.z);
		}
		 else if (Input.mousePosition.x < (Screen.width / 10) * 1) 
		{
			cameraReference.transform.position = new Vector3 (cameraReference.transform.position.x - Time.deltaTime * 20, 
				cameraReference.transform.position.y , cameraReference.transform.position.z);
		}
		if (Input.mousePosition.y > (Screen.height / 10) * 9) 
		{
			cameraReference.transform.position = new Vector3 (cameraReference.transform.position.x, 
				cameraReference.transform.position.y, cameraReference.transform.position.z  + Time.deltaTime * 20);
		}
		 else if (Input.mousePosition.y < (Screen.height / 10) * 1) 
		{
			cameraReference.transform.position = new Vector3 (cameraReference.transform.position.x, 
				cameraReference.transform.position.y, cameraReference.transform.position.z  - Time.deltaTime * 20);
		}
		
	}

	void RotateAroundCameraControl()
	{
		Node center = StageData.currentInstance.GetNodeFromPosition(cameraReference.ScreenToWorldPoint (
													  new Vector3(Screen.width / 2, Screen.height / 2, 0f)));

		angleForce = Vector2.Distance(Input.mousePosition, new Vector2 (Screen.width / 2, Screen.height / 2));
		print (angleForce);

		cameraReference.transform.RotateAround (center.position, new Vector3 (0f, 1.0f, 0.0f), angleForce);


	}



	// Update is called once per frame
	void Update () {

		ZoomCameraControl ();
		PositionCameraControl ();
		if (Input.GetMouseButtonDown (1)) 
		{
			RotateAroundCameraControl ();
		}

		//Testeo de que funciona mover la camara y tal...
		/*cameraReference.gameObject.transform.position = new Vector3(cameraReference.gameObject.transform.position.x,
			cameraReference.gameObject.transform.position.y + 1, cameraReference.gameObject.transform.position.z);*/
		//cameraHolder.transform.position.y++;
	}
}
