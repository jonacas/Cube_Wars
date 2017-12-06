using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aldeano : Unidad {

    private int SALUD_MAX_ALDEANO = 100;
    private int VISION_ALDEANO = 4;
    private int CASILLAS_MOVIMIENTO_ALDEANO = 4;

    //ACCIONES DE LA UNIDAD
    private const int ACCION_MOVER = 0;
    private const int ACCION_RECOLECTAR = 1;
    private const int ACCION_CONSTRUIR = 2;

    void Awake()
    {
        //Nodo = StageData.currentInstance.GetNodeFromPosition(transform.position);
        saludMaxima = SALUD_MAX_ALDEANO;
        Vida = SALUD_MAX_ALDEANO;
        vision = VISION_ALDEANO;
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
