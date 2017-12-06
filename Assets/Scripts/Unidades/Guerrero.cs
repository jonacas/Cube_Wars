using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guerrero : Unidad {

    public const int ALCANCE_GUERRERO = 2;
    public const int VISION_GUERRERO = 3;
    public const int ATAQUE_GUERRERO = 10;
    public const int SALUD_MAX_GUERRERO = 50;
    public const int DEFENSA_MAX_GUERRERO = 10;

    //ACCIONES DE LA UNIDAD
    private const int ACCION_MOVER = 0;
    private const int ACCION_ATACAR = 1;

    void Awake()
    {        
        acciones = new List<Accion>();
        acciones.Add(GetComponent<MoverUnidad>());
        acciones.Add(GetComponent<Atacar>());
        vision = VISION_GUERRERO;
        saludMaxima = SALUD_MAX_GUERRERO;
        Vida = SALUD_MAX_GUERRERO;
        defensaMaxima = DEFENSA_MAX_GUERRERO;
        Defensa = DEFENSA_MAX_GUERRERO;
        danyo = ATAQUE_GUERRERO;
        idUnidad = TipoUnidad.Warrior;
        IdJugador = 2;
    }

    /*private void Start()
    {
        Nodo = StageData.currentInstance.GetNodeFromPosition(transform.position);
    }*/

    void Update()
    {
            Nodo = StageData.currentInstance.GetNodeFromPosition(transform.position);
            Nodo.unidad = this;
    }


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
