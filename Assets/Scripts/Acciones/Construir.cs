﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Construir : Accion {

    private const float OFFSET_Y = 2F;


    #region COSTES
    public const int COSTE_PA_EDIFICIO_RECOLECCION = 30;
    public const int COSTE_MADERA_EDIFICIO_RECOLECCION = 100;
    public const int COSTE_ROCA_EDIFICIO_RECOLECCION = 30;

    public const int COSTE_PA_TORRE_DEFENSA = 30;
    public const int COSTE_MADERA_TORRE_DEFENSA = 20;
    public const int COSTE_ROCA_TORRE_DEFENSA = 20;
    public const int COSTE_METAL_TORRE_DEFENSA = 50;
    #endregion

    public List<Node> NodosAlAlcance;

    GameObject fantasmaTorre;
    GameObject fantasmaEdificioRecoleccion;

    void Awake()
    {
        //aqui se deben coger los fantasmas que se mostraran para no instanciarlos mas tarde
        costeAccion = 50;
        m_Unidad = GetComponent<Unidad>();
        Alcance = 3;
        //NodosAlAlcance = Control.GetNodosAlAlcance(m_Unidad.Nodo, Alcance);
        idAccion = AccionID.build;
    }

    /// <summary>
    /// Esta funcion muestra una version transparente de lo que se va a construir sobre el nodo en el que se encuentra el raton
    /// solo si está dentro del alcance. Llamar a SetAlcance() antes.
    /// </summary>
    /// <param name="nodo">Nodo sobre el que esta el raton</param>
    public void MostrarFantasmas(Node nodo)
    {
        Debug.LogError("ERROR EN ACCION CONSTRUIR: La funcion aun no esta del todo implementada");
        if (NodosAlAlcance != null)
        {
            if (NodosAlAlcance.Contains(nodo))
            {
                //si no hay una unidad ocupando la celda
                if (nodo.unidad == null)
                {
                    //si no hay un recurso, se muestra la torre
                    if (nodo.resourceType != null)
                    {
                        fantasmaTorre.SetActive(true);
                        fantasmaTorre.transform.position = nodo.position + new Vector3(0, OFFSET_Y, 0);
                    }
                    //si hay un recurso, se muestra el edificio de recoleccion
                    else
                    {
                        fantasmaEdificioRecoleccion.SetActive(true);
                        fantasmaEdificioRecoleccion.transform.position = nodo.position + new Vector3(0, OFFSET_Y, 0);
                    }
                }
            }
            else
            {
                //desactivamos fantasmas
                fantasmaTorre.SetActive(false);
                fantasmaEdificioRecoleccion.SetActive(false);
            }
        }

        else
            Debug.LogError("ERROR EN ACCION CONSTRUIR: Nodos al alcance de la accion es nulo");
    }

    /// <summary>
    /// Comprueba si el jugador tiene puntos suficientes y ejecuta la accion
    /// </summary>
    /// <param name="j">Jugador que realiza la accion</param>
    /// <returns></returns>
    public bool Ejecutar(Node n)
    {
        NodosAlAlcance = Control.GetNodosAlAlcance(m_Unidad.Nodo, Alcance);
        //SeleccionarResaltoDeCasilla();
        //si el nodo esta al alcance
        if (NodosAlAlcance.Contains(n))
        {
            if (n.resourceType == TipoRecurso.NullResourceType)
            {
                if (true/*Partida.GetPartidaActual().Jugadores[m_Unidad.IdJugador].RestarPuntosDeAccion(costeAccion)*/)
                {
                    Instantiate(StageData.currentInstance.TowerPrefab, n.position, StageData.currentInstance.TowerPrefab.transform.rotation);
                    CancelarAccion();
                    return true;
                }
            }
            else
            {//por si queremos poner costes distintos
                if (Partida.GetPartidaActual().Jugadores[m_Unidad.IdJugador].RestarPuntosDeAccion(costeAccion))
                {
                    Instantiate(StageData.currentInstance.ResourceBuildPrefab, n.position, StageData.currentInstance.ResourceBuildPrefab.transform.rotation);
                    CancelarAccion();
                    return true;
                }
            }

        }
        CancelarAccion();
        return false;
    }
    
    public override void CancelarAccion()
    {
        fantasmaTorre.SetActive(false);
        fantasmaEdificioRecoleccion.SetActive(false);
        m_Unidad.QuitarResaltoCasillasAlAlcance(NodosAlAlcance);
    }

    public override void EmpezarAccion()
    {
        SeleccionarResaltoDeCasilla();
        m_Unidad.ResaltarCasillasAlAlcance(NodosAlAlcance);
    }

    public override void SeleccionarResaltoDeCasilla()
    {
        NodosAlAlcance = Control.GetNodosAlAlcance(m_Unidad.Nodo, Alcance);
        for (int i = NodosAlAlcance.Count - 1; i >= 0; i--)
        {
            if (NodosAlAlcance[i].unidad != null ||
                NodosAlAlcance[i].resourceType != TipoRecurso.NullResourceType)
            {
                NodosAlAlcance.Remove(NodosAlAlcance[i]);
            }
        }
    }
}
