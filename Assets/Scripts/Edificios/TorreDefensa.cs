using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorreDefensa : Unidad{

    const int ACCION_ATACAR = 0;
    
    void Awake()
    {
        acciones = new List<Accion>();
        acciones.Add(GetComponent<Atacar>());
        saludMaxima = StageData.SALUD_MAX_TORRE_DEFENSIVA;
        Vida = StageData.SALUD_MAX_TORRE_DEFENSIVA;
        defensaMaxima = StageData.DEFENSA_MAX_TORRE_DEFENSIVA;
        Defensa = StageData.DEFENSA_MAX_TORRE_DEFENSIVA;
		idUnidad = TipoUnidad.DefensiveBuilding;  
		Nodo = StageData.currentInstance.GetNodeFromPosition(transform.position);
        Debug.Log("Pos Nodo Centro: " + Nodo.fil + " , " + Nodo.col );
        Debug.Log("Pos Nodo 0,0: " + StageData.currentInstance.grafoTotal[0,0].position.x + " , " +
                                     StageData.currentInstance.grafoTotal[0, 0].position.z);
        Debug.Log("Pos Nodo ExtremoOpuesto: " + StageData.currentInstance.grafoTotal[49, 49].position.x + " , " +
                                     StageData.currentInstance.grafoTotal[49, 49].position.z);
        //StageData.currentInstance.SetInfluenceToNode(Node.stepsInfluenceDefensiveBuilding, Nodo, StageData.currentInstance.GetPartidaActual().JugadorActual.idJugador);
        //linea innecesaria pa al version final?
    }

    public void AtacarAlInicioTurno()
    {
        List<Node> nodos =  Control.GetNodosAlAlcance(Nodo, acciones[ACCION_ATACAR].Alcance);

        foreach (Node n in nodos)
        {
            if (n.unidad != null && n.unidad.IdJugador == IdJugador)
            {
                n.unidad.RecibirAtaque(StageData.ATAQUE_TORRE_DEFENSIVA);
            }
        }

    }
}
