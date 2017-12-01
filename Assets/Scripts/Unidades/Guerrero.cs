using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guerrero : Unidad {

    public const int ALCANCE_GUERRERO = 2;
    public const int VISION_GUERRERO = 3;
    public const int ATAQUE_GUERRERO = 10;
    public const int SALUD_MAX_GUERRERO = 50;
    public const int DEFENSA_MAX_GUERRERO = 10;

    Node nodo;


    void Awake()
    {
        nodo = StageData.currentInstance.GetNodeFromPosition(transform.position);
        acciones = new List<Accion>();
        vision = VISION_GUERRERO;
        saludMaxima = SALUD_MAX_GUERRERO;
        Vida = SALUD_MAX_GUERRERO;
        defensaMaxima = DEFENSA_MAX_GUERRERO;
        Defensa = DEFENSA_MAX_GUERRERO;
        idUnidad = TipoUnidad.Explorer;
    }

}
