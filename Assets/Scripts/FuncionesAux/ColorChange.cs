using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChange : MonoBehaviour {

    public Material material;
	// Use this for initialization
	void Awake () {

        material.color = StageData.currentInstance.colores[StageData.currentInstance.GetPartidaActual().JugadorActual.idJugador];
	}
}
