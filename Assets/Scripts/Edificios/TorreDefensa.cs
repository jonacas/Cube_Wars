using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorreDefensa : Unidad{

    const int ACCION_ATACAR = 0;

    Node nodo;

    void Awake()
    {
        acciones = new List<Accion>();
        acciones.Add(GetComponent<Atacar>());
        saludMaxima = StageData.SALUD_MAX_TORRE_DEFENSIVA;
        Vida = StageData.SALUD_MAX_TORRE_DEFENSIVA;
        defensaMaxima = StageData.DEFENSA_MAX_TORRE_DEFENSIVA;
        Defensa = StageData.DEFENSA_MAX_TORRE_DEFENSIVA;
		idUnidad = TipoUnidad.DefensiveBuilding;  
		//nodo = StageData.currentInstance.GetNodeFromPosition(transform.position);
		//linea innecesaria pa al version final?
    }

    public void AtacarAlInicioTurno()
    {
        List<Node> nodos =  Control.GetNodosAlAlcance(nodo, acciones[ACCION_ATACAR].Alcance);

        foreach (Node n in nodos)
        {
            if (n.unidad != null && n.unidad.IdJugador == IdJugador)
            {
                n.unidad.RecibirAtaque(StageData.ATAQUE_TORRE_DEFENSIVA);
            }
        }

    }
}
