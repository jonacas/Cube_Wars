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
        saludMaxima = SALUD_MAX_ALDEANO;
        Vida = SALUD_MAX_ALDEANO;
        vision = VISION_ALDEANO;
        acciones = new List<Accion>();
        acciones.Add(this.GetComponent<MoverUnidad>());
        idUnidad = GlobalData.ID_EXPLORADOR;

        //FALTA RELLENAR INFLUENCIAS
    }

}
