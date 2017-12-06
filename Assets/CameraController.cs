using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    private const float EDGE_PANNING_SPEED = 1.25f;
    private const float RIGHT_CLICK_TURN_SPEED = 10f;
    private const float EDGE_PANNING_THS_HORIZONTAL = 0.475f;
    private const float EDGE_PANNING_THS_VERTICAL = 0.45f;

    private float mousePositionPercentX;
    private float mousePositionPercentY;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButton(1))
        {
            UpdateRightClickTurning();
        }
        else
        {
            UpdateEdgePanning();
        }

	}
    void UpdateRightClickTurning()
    {
        transform.Rotate(0,RIGHT_CLICK_TURN_SPEED * Input.GetAxis("Mouse X"), 0);
    }
    void UpdateEdgePanning()
    {
        mousePositionPercentX = (Input.mousePosition.x / Screen.width) - 0.5f;
        mousePositionPercentY = (Input.mousePosition.y / Screen.height) - 0.5f;
        if (Mathf.Abs(mousePositionPercentX) > EDGE_PANNING_THS_HORIZONTAL)
        {
            transform.Translate(Vector3.right * mousePositionPercentX * EDGE_PANNING_SPEED);
        }
        if (Mathf.Abs(mousePositionPercentY) > EDGE_PANNING_THS_VERTICAL)
        {
            transform.Translate(Vector3.forward * mousePositionPercentY * EDGE_PANNING_SPEED);
        }
    }
}
