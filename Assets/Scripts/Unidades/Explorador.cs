using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explorador : Unidad {

    const int ACCION_MOVER = 0;

	// Use this for initialization
	void Awake () {
        saludMaxima = StageData.SALUD_MAX_EXPLORADOR;
        Vida = StageData.SALUD_MAX_EXPLORADOR;
        acciones = new List<Accion>();
        acciones.Add(this.GetComponent<MoverUnidad>());
        idUnidad = TipoUnidad.Explorer;
        IdJugador = 0;

        //FALTA RELLENAR INFLUENCIAS
	}

    /*private void Start()
    {
        //Nodo = StageData.currentInstance.GetNodeFromPosition(transform.position);
    }*/

    /*void Update()
    {
            Nodo = StageData.currentInstance.GetNodeFromPosition(transform.position);
            Nodo.unidad = this;
    }*/

    public void AccionMover(List<Vector3> camino)
	{
		MoverUnidad mv = (MoverUnidad)acciones [ACCION_MOVER];
        print(mv == null);
		//mv.Ejecutar (StageData.currentInstance.GetNodeFromPosition(camino[camino.Count-1]), camino);
	}

    public override void SolicitarYRecorrerCamino(Vector3 final)
    {
        base.SolicitarYRecorrerCamino(final);
        StartCoroutine("EsperarCamino");
    }

    public IEnumerator EsperarCamino()
    {
        while (!caminoListo)
            yield return null;
        print("Espera camino terminada");
        AccionMover(caminoActual);
    }
}
