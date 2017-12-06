using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementBehaviour : MonoBehaviour {


	public Camera cameraReference;

	Transform initialCameraLocation;
	float minFieldOfView = 30f;
	float maxFieldOfView;

	public float angleForce = 0.0f;
	Quaternion rotationCamera;
	public float cameraTraslationSpeed = 20f;

	public GameObject limitPositionMinimumXZ;
	public GameObject limitPositionMaximumXZ;

	bool turnEnabled = false;

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
			
				/*cameraReference.transform.position = new Vector3 (cameraReference.transform.position.x + Time.deltaTime * 20, 
					cameraReference.transform.position.y , cameraReference.transform.position.z);
				*/
			cameraReference.transform.position += cameraReference.transform.right * Time.deltaTime * cameraTraslationSpeed;


		}
		 else if (Input.mousePosition.x < (Screen.width / 10) * 1) 
		{
			
				/*
				cameraReference.transform.position = new Vector3 (cameraReference.transform.position.x - Time.deltaTime * 20, 
					cameraReference.transform.position.y , cameraReference.transform.position.z);
				*/
			cameraReference.transform.position -= cameraReference.transform.right * Time.deltaTime * cameraTraslationSpeed;

		}
		if (Input.mousePosition.y > (Screen.height / 10) * 9) 
		{
				/*
				cameraReference.transform.position = new Vector3 (cameraReference.transform.position.x, 
					cameraReference.transform.position.y, cameraReference.transform.position.z + Time.deltaTime * 20);
				*/
				rotationCamera = cameraReference.transform.localRotation;

				cameraReference.transform.rotation = new Quaternion (0f, cameraReference.transform.localRotation.y,
													 cameraReference.transform.rotation.z, cameraReference.transform.rotation.w);
				
			cameraReference.transform.position += cameraReference.transform.forward * Time.deltaTime * cameraTraslationSpeed;

				cameraReference.transform.rotation = rotationCamera;

		}
		 else if (Input.mousePosition.y < (Screen.height / 10) * 1) 
		{
				/*
				cameraReference.transform.position = new Vector3 (cameraReference.transform.position.x, 
					cameraReference.transform.position.y, cameraReference.transform.position.z - Time.deltaTime * 20);
				*/
				rotationCamera = cameraReference.transform.localRotation;

				cameraReference.transform.rotation = new Quaternion (0f, cameraReference.transform.localRotation.y,
					cameraReference.transform.rotation.z, cameraReference.transform.rotation.w);

			cameraReference.transform.position -= cameraReference.transform.forward * Time.deltaTime * cameraTraslationSpeed;

				cameraReference.transform.rotation = rotationCamera;

		}

		if (cameraReference.transform.position.z < limitPositionMinimumXZ.transform.position.z) 
		{
			cameraReference.transform.position = new Vector3 (cameraReference.transform.position.x, 
				cameraReference.transform.position.y, limitPositionMinimumXZ.transform.position.z);
		}
		if (cameraReference.transform.position.z > limitPositionMaximumXZ.transform.position.z) 
		{
			cameraReference.transform.position = new Vector3 (cameraReference.transform.position.x, 
				cameraReference.transform.position.y, limitPositionMaximumXZ.transform.position.z);
		}
		if (cameraReference.transform.position.x < limitPositionMinimumXZ.transform.position.x) 
		{
			cameraReference.transform.position = new Vector3 (limitPositionMinimumXZ.transform.position.x, 
				cameraReference.transform.position.y , cameraReference.transform.position.z);
		}
		if (cameraReference.transform.position.x > limitPositionMaximumXZ.transform.position.x) 
		{
			cameraReference.transform.position = new Vector3 (limitPositionMaximumXZ.transform.position.x, 
				cameraReference.transform.position.y , cameraReference.transform.position.z);
		}


	}

	void RotateAroundCameraControl()
	{
		if (turnEnabled) 
		{
			Node center = StageData.currentInstance.GetNodeFromPosition(cameraReference.ScreenToWorldPoint (
				new Vector3(Screen.width / 2, Screen.height / 2, 0f)));

			angleForce = Mathf.Abs(Vector2.Distance(Input.mousePosition, new Vector2 (Screen.width / 2, Screen.height / 2))) / 200f;
			//print (angleForce);

			if (Input.mousePosition.x < Screen.width / 2) {angleForce  = -angleForce;}

			cameraReference.transform.RotateAround (center.position, new Vector3 (0f, 1.0f, 0.0f), angleForce);
		}

	}



	// Update is called once per frame
	void Update () {

		ZoomCameraControl ();
		RotateAroundCameraControl ();
		if (Input.GetMouseButton(1)) 
		{
			turnEnabled = true;	
		} 
		else 
		{
			turnEnabled = false;	
			PositionCameraControl ();
		}


		//Testeo de que funciona mover la camara y tal...
		/*cameraReference.gameObject.transform.position = new Vector3(cameraReference.gameObject.transform.position.x,
			cameraReference.gameObject.transform.position.y + 1, cameraReference.gameObject.transform.position.z);*/
		//cameraHolder.transform.position.y++;
	}
}
