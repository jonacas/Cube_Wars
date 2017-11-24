using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explorador : Unidad {

    private int SALUD__MAX_EXPLORADOR = 100;
    private int VISION_EXPLORADOR = 7;
    private int CASILLAS_MOVIMIENTO_EXPLORADOR = 4;

	// Use this for initialization
	void Awake () {
        saludMaxima = SALUD__MAX_EXPLORADOR;
        Vida = SALUD__MAX_EXPLORADOR;
        vision = VISION_EXPLORADOR;
        acciones = new List<Accion>();
        acciones.Add(this.GetComponent<MoverUnidad>());
        idUnidad = GlobalData.ID_EXPLORADOR;

        //FALTA RELLENAR INFLUENCIAS
	}
}
