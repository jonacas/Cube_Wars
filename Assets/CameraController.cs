using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    private float EDGE_PANNING_MAX_X = 50;
    private float EDGE_PANNING_MIN_X = -50;
    private float EDGE_PANNING_MAX_Y = 50;
    private float EDGE_PANNING_MIN_Y = -50;

    private float EDGE_PANNING_LIMIT_ABSOLUTE_Y = 30;
    private const float EDGE_PANNING_SPEED = 90f;                           // Velocidad del edge panning
    private const float RIGHT_CLICK_TURN_SPEED = 100f;                      // Velocidad de giro de la camara
    private const float EDGE_PANNING_THS_HORIZONTAL = 0.475f;               // Posicion del raton necesaria para mover la pantalla en X (de 0 a 0.5)
    private const float EDGE_PANNING_THS_VERTICAL = 0.45f;                  // Posicion del raton necesaria para mover la pantalla en Y (de 0 a 0.5)
    private const float ZOOM_SPEED = 2f;                                    // Velocidad de zoom
    private const float ZOOM_MIN_HEIGHT = 3.5f;                                // Altura minima del zoom
    private const float ZOOM_HEIGHT_SCALING = 25f;                          // Escalado de altura del zoom (el zoom maximo sera, posicion minima + escalado)
    private const float ZOOM_MIN_ANGLE = 30f;                               // Angulo minimo de la camara de zoom
    private const float ZOOM_ANGLE_SCALING = 30f;                           // Escalado de angulo del zoom (el angulo maximo sera, angulo minimo + escalado)

    private float zoomTarget_T = 0.5f;
    private float zoomCurrent_T = 0.5f;

    private float mousePositionPercentX;
    private float mousePositionPercentY;

    public Transform zoomTransform;

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
        UpdateZoom();

	}
    void UpdateZoom()
    {
        zoomTarget_T -= Input.GetAxis("Mouse ScrollWheel");
        zoomTarget_T = Mathf.Clamp01(zoomTarget_T);
        zoomCurrent_T = Mathf.MoveTowards(zoomCurrent_T, zoomTarget_T, Time.deltaTime * ZOOM_SPEED);
        zoomTransform.localPosition = new Vector3(0,ZOOM_MIN_HEIGHT+(zoomCurrent_T*ZOOM_HEIGHT_SCALING),0);
        zoomTransform.localRotation = Quaternion.Euler(ZOOM_MIN_ANGLE + zoomCurrent_T * ZOOM_ANGLE_SCALING,0,0);
    }
    void UpdateRightClickTurning()
    {
        transform.Rotate(0,RIGHT_CLICK_TURN_SPEED * Input.GetAxis("Mouse X") * Time.deltaTime, 0);
    }
    void UpdateEdgePanning()
    {
        mousePositionPercentX = (Input.mousePosition.x / Screen.width) - 0.5f;
        mousePositionPercentY = (Input.mousePosition.y / Screen.height) - 0.5f;
        if (Mathf.Abs(mousePositionPercentX) > EDGE_PANNING_THS_HORIZONTAL)
        {
            mousePositionPercentX = Mathf.MoveTowards(mousePositionPercentX, 0, EDGE_PANNING_THS_HORIZONTAL);
            transform.Translate(Vector3.right * mousePositionPercentX * EDGE_PANNING_SPEED * Time.deltaTime);
        }
        if (Mathf.Abs(mousePositionPercentY) > EDGE_PANNING_THS_VERTICAL)
        {
            mousePositionPercentY = Mathf.MoveTowards(mousePositionPercentY, 0, EDGE_PANNING_THS_VERTICAL);
            transform.Translate(Vector3.forward * mousePositionPercentY * EDGE_PANNING_SPEED * Time.deltaTime);
        }
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, EDGE_PANNING_MIN_X, EDGE_PANNING_MAX_X),0, Mathf.Clamp(transform.position.z, EDGE_PANNING_MIN_Y, EDGE_PANNING_MAX_Y));
    }
}
