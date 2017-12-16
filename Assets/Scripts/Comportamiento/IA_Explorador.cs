using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IA_Explorador : Unidad {

    private bool heLlegado, listo;
    private List<Vector3> caminoTotalACapital;
    int posActual;

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
        //espera a que el camino este listo y lo guarda
        while (!caminoListo)
            yield return null;
        caminoTotalACapital = caminoActual;
        posActual = 0;
        //print("Espera camino terminada");
        //AccionMover(caminoActual);
    }

    public void AccionAtacar(Node objetivo)
    {
        Atacar at = (Atacar)acciones[ACCION_ATACAR];
        print(at == null);
        //at.Ejecutar(objetivo.unidad);
    }

    public void SetDestino(Node destino)
    {
        heLlegado = false;
        SolicitarYRecorrerCamino(destino.position);
    }

    public void AvanzarHaciaDestino(ref int puntosDisponibles)
    {
        //si hay unidades enemigas al alcance (guerreros y torres de defensa), las ataca si son del objetivo
        listo = false;
        List<Node> alcance;
        alcance = Control.GetNodosAlAlcance(StageData.currentInstance.GetNodeFromPosition(this.transform.position), ALCANCE_GUERRERO);

        for (int i = 0; i < alcance.Count; i++)
        {
            if (alcance[i].unidad != null  && alcance[i].unidad.IdJugador == StageData.currentInstance.GetPartidaActual().Jugadores[IdJugador].rolGuerrero.objetivoActual)
            {
                if (alcance[i].unidad.IdUnidad == TipoUnidad.Warrior || alcance[i].unidad.IdUnidad == TipoUnidad.DefensiveBuilding || alcance[i].unidad.IdUnidad == TipoUnidad.Capital)
                {
                    Atacar at = (Atacar)acciones[ACCION_ATACAR];
                    if (at.Ejecutar(alcance[i]))
                    {
						puntosDisponibles -= StageData.COSTE_PA_ATACAR;
                        listo = true;
                        return;
                    }
                }
            }
        }

        //si no atacamos, movemos
        caminoActual = caminoTotalACapital.GetRange(posActual, ALCANCE_GUERRERO - 1);

        //comprobamos a que posiciones podemos movernos
        Vector3 destino = Vector3.zero;
        int incrementoPos = 0;
        for (int i = caminoActual.Count - 1; i >= 0; i--)
        {
            if (StageData.currentInstance.GetNodeFromPosition(caminoActual[i]).unidad == null && StageData.currentInstance.GetNodeFromPosition(caminoActual[i]).resourceType == TipoRecurso.NullResourceType)
            {
                destino = caminoActual[i];
                incrementoPos = i;
                break;
            }
        }

        //si no puede moverse, ha llegado
        if (destino == Vector3.zero)
        {
            heLlegado = true;
            listo = true;
            return;
        }

        //si puede moverse, lo hace
        MoverUnidad mv = (MoverUnidad)acciones[ACCION_MOVER];
        if (mv.Ejecutar(StageData.currentInstance.GetNodeFromPosition(destino)))
        {
            posActual += incrementoPos;
            puntosDisponibles -= StageData.COSTE_PA_MOVER_UNIDAD;
        }
        listo = true;
    }

    public bool HaLlegado()
    {
        return heLlegado;
    }

    public bool AccionTerminada()
    {
        return listo;
    }

}
