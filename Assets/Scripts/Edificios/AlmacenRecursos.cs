using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlmacenRecursos : Unidad {

    // Use this for initialization
    void Awake()
    {
        saludMaxima = StageData.SALUD_MAX_RECOLECTOR;
        Vida = StageData.SALUD_MAX_RECOLECTOR;
        defensaMaxima = StageData.DEFENSA_MAX_RECOLECTOR;
        Defensa = StageData.DEFENSA_MAX_RECOLECTOR;
        idUnidad = TipoUnidad.Resource;
        acciones = new List<Accion>();
        acciones.Add(GetComponent<CrearUnidad>());
    }

    public void SumarRecursos() // COGE AL JUGADOR ACTUAL
    {
        //StageData.currentInstance.GetPartidaActual().JugadorActual.SumarRecursos(Nodo.resourceType, StageData.CANTIDAD_RECOLECTADA);
        StageData.currentInstance.GetPartidaActual().JugadorActual.SumarRecursos(TipoRecurso.AllTypeResource, StageData.CANTIDAD_RECOLECTADA);
    }
}
