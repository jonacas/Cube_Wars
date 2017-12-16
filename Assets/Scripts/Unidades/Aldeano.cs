using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aldeano : Unidad {

    const int ACCION_MOVER = 0;
    const int ACCION_CONSTRUIR = 1;

    void Awake()
    {
        //Nodo = StageData.currentInstance.GetNodeFromPosition(transform.position);
        saludMaxima = StageData.SALUD_MAX_ALDEANO;
        Vida = StageData.SALUD_MAX_ALDEANO;
        acciones = new List<Accion>();
        acciones.Add(this.GetComponent<MoverUnidad>());
        acciones.Add(this.GetComponent<Construir>());
        idUnidad = TipoUnidad.Worker;

        //FALTA RELLENAR INFLUENCIAS
    }

    /*void Update()
    {
        Nodo = StageData.currentInstance.GetNodeFromPosition(transform.position);
        Nodo.unidad = this;
    }*/

    public void AccionMover(List<Vector3> camino)
    {
        MoverUnidad mv = (MoverUnidad)acciones[ACCION_MOVER];
        print(mv == null);
       // mv.Ejecutar(StageData.currentInstance.GetNodeFromPosition(camino[camino.Count - 1]), camino);
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

    public void AccionConstruir(Node objetivo)
    {
        Construir co = (Construir)acciones[ACCION_CONSTRUIR];
        print(co == null);
        co.Ejecutar(objetivo);
    }

}
