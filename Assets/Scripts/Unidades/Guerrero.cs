using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guerrero : Unidad {

    const int ACCION_MOVER = 0;
    const int ACCION_ATACAR = 1;

    void Awake()
    {        
        acciones = new List<Accion>();
        acciones.Add(GetComponent<MoverUnidad>());
        acciones.Add(GetComponent<Atacar>());
        saludMaxima = StageData.SALUD_MAX_GUERRERO;
        Vida = StageData.SALUD_MAX_GUERRERO;
        defensaMaxima = StageData.DEFENSA_MAX_GUERRERO;
        Defensa = StageData.DEFENSA_MAX_GUERRERO;
        danyo = StageData.ATAQUE_GUERRERO;
        idUnidad = TipoUnidad.Warrior;
        IdJugador = 2;
        Nodo = StageData.currentInstance.GetNodeFromPosition(this.transform.position);
        Nodo.unidad = this;
    }

    /*private void Start()
    {
        Nodo = StageData.currentInstance.GetNodeFromPosition(transform.position);
    }*/

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

    public void AccionAtacar(Node objetivo)
    {
        Atacar at = (Atacar)acciones[ACCION_ATACAR];
        print(at == null);
        //at.Ejecutar(objetivo.unidad);
    }

}
