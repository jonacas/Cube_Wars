using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

    Camera cam;
    StageData SD;
    List<Node> validPositions = null;

    public Unidad unidadSeleccionada;

    ///VARIABLES AUXILIARES PARA ALMACENAR INSTANCIAS DE UNIDADES
    public Explorador explorador;

	// Use this for initialization
	void Start () {
        cam = this.GetComponent<Camera>();
        SD = StageData.currentInstance;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("ClickIzq"))
        {
            if (validPositions == null)
            {
                print("clickIzq");
                Node nodo = LanzaRaycast();
                SD.LimpiarGrafo(SD.CG.nodeMap);
                validPositions = Control.GetNodosAlAlcance(nodo, 2);
                SD.LimpiarGrafo(SD.CG.nodeMap);
            }
            else
            {
                
                Node nodo = LanzaRaycast();
                if (validPositions.Contains(nodo))
                {
                    print("Posicion valida");
                    explorador.SolicitarYRecorrerCamino(nodo.position);
                }
                validPositions = null;
            }
        }
	}

    /// <summary>
    /// Devuelve fila y columna correspondiente al click
    /// </summary>
    /// <returns></returns>
    private Node LanzaRaycast()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out hit);

        if (hit.collider != null)
        {
            
            return SD.GetNodeFromPosition(hit.point);
        }
        else
            return null;
    }

    private List<Vector3> getNodesPosition(List<Node> nodes)
    {
        List<Vector3> path = new List<Vector3>();
        foreach (Node n in nodes)
        {
            path.Add(n.position);
        }

        return path;

    }
}
