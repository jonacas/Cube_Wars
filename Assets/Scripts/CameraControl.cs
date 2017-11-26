using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

    Camera cam;
    StageData SD;
	// Use this for initialization
	void Start () {
        cam = this.GetComponent<Camera>();
        SD = StageData.currentInstance;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("ClickIzq"))
        {
            print("clickIzq");
            int[] filaColumna = LanzaRaycast();
            Node nodo = SD.CG.nodeMap[filaColumna[0], filaColumna[1]];
            SD.LimpiarGrafo(SD.CG.nodeMap);
            Control.GetNodosAlAlcance(nodo, 2);
            SD.LimpiarGrafo(SD.CG.nodeMap);
        }
	}

    /// <summary>
    /// Devuelve fila y columna correspondiente al click
    /// </summary>
    /// <returns></returns>
    private int[] LanzaRaycast()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out hit);

        if (hit.collider != null)
        {
            string[] nombreFrag = hit.collider.gameObject.name.Split('X');
            print(nombreFrag[0] + nombreFrag[1] + nombreFrag[2]);


            return new int[2] { System.Convert.ToInt32(nombreFrag[1]), System.Convert.ToInt32(nombreFrag[2]) };
        }
        else
            return null;
    }
}
