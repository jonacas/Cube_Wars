using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChange : MonoBehaviour {

	public Renderer materialReference;
	// Use this for initialization
	void Start () {

		materialReference.materials[0] = new Material(Shader.Find("Diffuse"));
		materialReference.materials [0].color = StageData.currentInstance.colores [StageData.currentInstance.GetPartidaActual ().JugadorActual.idJugador].color;
	}
}
