using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explorador : Unidad {

    private int SALUD_MAX_EXPLORADOR = 100;
    private int VISION_EXPLORADOR = 7;
    private int CASILLAS_MOVIMIENTO_EXPLORADOR = 4;

    //ACCIONES DE LA UNIDAD
    private const int ACCION_MOVER = 0;

	// Use this for initialization
	void Awake () {
        saludMaxima = SALUD_MAX_EXPLORADOR;
        Vida = SALUD_MAX_EXPLORADOR;
        vision = VISION_EXPLORADOR;
        acciones = new List<Accion>();
        acciones.Add(this.GetComponent<MoverUnidad>());
        idUnidad = GlobalData.ID_EXPLORADOR;

        //FALTA RELLENAR INFLUENCIAS
	}

	public void AccionMover(List<Vector3> camino)
	{
		MoverUnidad mv = (MoverUnidad)acciones [ACCION_MOVER];
		mv.Ejecutar (this.gameObject, camino);

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
        AccionMover(caminoActual);
    }
}
