using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorreDefensa : Unidad{

    public const int ALCANCE_TORRE_DEFENSA = 3;
    public const int VISION_TORRE_DEFENSA = 5;
    public const int ATAQUE_TORRE_DEFENSA = 20;
    public const int SALUD_MAX_TORRE_DEFENSA = 200;
    public const int DEFENSA_MAX_TORRE_DEFENSA = 100;

    Node nodo;

    void Awake()
    {
        nodo = StageData.currentInstance.GetNodeFromPosition(transform.position);
        acciones = null;
        vision = VISION_TORRE_DEFENSA;
        saludMaxima = SALUD_MAX_TORRE_DEFENSA;
        Vida = SALUD_MAX_TORRE_DEFENSA;
        defensaMaxima = DEFENSA_MAX_TORRE_DEFENSA;
        Defensa = DEFENSA_MAX_TORRE_DEFENSA;
        idUnidad = TipoUnidad.DefensiveBuilding;
    }

    public void AtacarAlInicioTurno()
    {
        List<Node> nodos =  Control.GetNodosAlAlcance(nodo, ALCANCE_TORRE_DEFENSA);

        foreach (Node n in nodos)
        {
            if (n.unidad != null)
            {
                n.unidad.RecibirAtaque(ATAQUE_TORRE_DEFENSA);
            }
        }

    }
}
