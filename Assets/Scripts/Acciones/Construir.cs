﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Construir : Accion
{

    private const float OFFSET_Y = 2F;

    public List<Node> NodosAlAlcance;

    GameObject fantasmaTorre;
    GameObject fantasmaEdificioRecoleccion;

    void Awake()
    {
        //aqui se deben coger los fantasmas que se mostraran para no instanciarlos mas tarde
        m_Unidad = GetComponent<Unidad>();
        Alcance = 4;
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
                    if (nodo.resourceType != TipoRecurso.NullResourceType)
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
        VerNodosAlAlcance();
        //SeleccionarResaltoDeCasilla();
        //si el nodo esta al alcance
        if (NodosAlAlcance.Contains(n))
        {
            if (n.resourceType == TipoRecurso.NullResourceType)
            {
                if (true/*StageData.currentInstance.GetPartidaActual().Jugadores[m_Unidad.IdJugador].RestarPuntosDeAccion(StageData.COSTE_PA_CONSTRUIR_TORRE)*/)
                {
                    StageData.currentInstance.GetPartidaActual().JugadorActual.TorresDefensa++;
					GameObject tower = Instantiate(StageData.currentInstance.TowerPrefab, n.position, StageData.currentInstance.TowerPrefab.transform.rotation);
					StageData.currentInstance.GetPartidaActual ().JugadorActual.edificios.Add (tower.GetComponent<TorreDefensa>());
					StageData.currentInstance.RemoveResourceModel (n.position);
                    SetUnidadANodoYViceversa(tower.GetComponent<Unidad>());
                    tower.GetComponent<Unidad>().IdJugador = StageData.currentInstance.GetPartidaActual().JugadorActual.idJugador;
                    StageData.currentInstance.GetPartidaActual().JugadorActual.edificios.Add(tower.GetComponent<Unidad>());
                    CancelarAccion();
                    return true;
                }
            }
            else
            {//por si queremos poner costes distintos
                if (true/*StageData.currentInstance.GetPartidaActual().Jugadores[m_Unidad.IdJugador].RestarPuntosDeAccion(StageData.COSTE_PA_CONSTRUIR_RECURSOS)*/)
                {
                    StageData.currentInstance.GetPartidaActual().JugadorActual.EdificiosRecoleccion++;
                    switch (n.resourceType)
                    {
                        case TipoRecurso.AllTypeResource:
                        {
                                GameObject almacen = Instantiate(StageData.currentInstance.ResourceBuildPrefab,
                                    n.position, StageData.currentInstance.ResourceBuildPrefab.transform.rotation);
                                StageData.currentInstance.RemoveResourceModel(n.position);
                                SetUnidadANodoYViceversa(almacen.GetComponent<Unidad>());
                                almacen.GetComponent<Unidad>().IdJugador = StageData.currentInstance.GetPartidaActual().JugadorActual.idJugador;
                                StageData.currentInstance.GetPartidaActual().JugadorActual.edificios.Add(almacen.GetComponent<Unidad>());
                                break;
                        }
                        case TipoRecurso.Food:
                        {
                              GameObject almacen = Instantiate(StageData.currentInstance.ResourceBuildPrefab,
                                    n.position, StageData.currentInstance.ResourceBuildPrefab.transform.rotation);
                                StageData.currentInstance.RemoveResourceModel(n.position);
                                SetUnidadANodoYViceversa(almacen.GetComponent<Unidad>());
                                almacen.GetComponent<Unidad>().IdJugador = StageData.currentInstance.GetPartidaActual().JugadorActual.idJugador;
                                StageData.currentInstance.GetPartidaActual().JugadorActual.edificios.Add(almacen.GetComponent<Unidad>());
                                break;
                        }
                        case TipoRecurso.Steel:
                            {
                                GameObject almacen = Instantiate(StageData.currentInstance.MetalBuildPrefab,
                                    n.position, StageData.currentInstance.ResourceBuildPrefab.transform.rotation);
                                StageData.currentInstance.RemoveResourceModel(n.position);
                                SetUnidadANodoYViceversa(almacen.GetComponent<Unidad>());
                                almacen.GetComponent<Unidad>().IdJugador = StageData.currentInstance.GetPartidaActual().JugadorActual.idJugador;
                                StageData.currentInstance.GetPartidaActual().JugadorActual.edificios.Add(almacen.GetComponent<Unidad>());
                                break;
                            }
                        case TipoRecurso.Wood:
                            {
                                GameObject almacen = Instantiate(StageData.currentInstance.WoodBuildPrefab,
                                    n.position, StageData.currentInstance.ResourceBuildPrefab.transform.rotation);
                                StageData.currentInstance.RemoveResourceModel(n.position);
                                SetUnidadANodoYViceversa(almacen.GetComponent<Unidad>());
                                almacen.GetComponent<Unidad>().IdJugador = StageData.currentInstance.GetPartidaActual().JugadorActual.idJugador;
                                StageData.currentInstance.GetPartidaActual().JugadorActual.edificios.Add(almacen.GetComponent<Unidad>());
                                break;
                            }
                        case TipoRecurso.Rock:
                            {
                                GameObject almacen = Instantiate(StageData.currentInstance.StoneBuildPrefab,
                                    n.position, StageData.currentInstance.ResourceBuildPrefab.transform.rotation);
                                StageData.currentInstance.RemoveResourceModel(n.position);
                                SetUnidadANodoYViceversa(almacen.GetComponent<Unidad>());
                                almacen.GetComponent<Unidad>().IdJugador = StageData.currentInstance.GetPartidaActual().JugadorActual.idJugador;
                                StageData.currentInstance.GetPartidaActual().JugadorActual.edificios.Add(almacen.GetComponent<Unidad>());
                                break;
                            }
                    }                
                    CancelarAccion();
                    return true;
                }
            }

        }
        CancelarAccion();
        return false;
    }

    void SetUnidadANodoYViceversa(Unidad unidad)
    {
        Vector3 aux = unidad.transform.position;
        Node nodoActual = StageData.currentInstance.GetNodeFromPosition(unidad.transform.position);
        unidad.Nodo = nodoActual;
        nodoActual.unidad = unidad;
        StageData.currentInstance.SetInfluenceToNode(Node.stepsInfluenceDefensiveBuilding, nodoActual, StageData.currentInstance.GetPartidaActual().JugadorActual.idJugador);
    }

    public override void CancelarAccion()
    {
		if (fantasmaTorre != null && fantasmaEdificioRecoleccion != null) 
		{
			fantasmaTorre.SetActive(false);
			fantasmaEdificioRecoleccion.SetActive(false);
		}      
        m_Unidad.QuitarResaltoCasillasAlAlcance(NodosAlAlcance);
    }

    public override void EmpezarAccion()
    {
        VerNodosAlAlcance();
        m_Unidad.ResaltarCasillasAlAlcance(NodosAlAlcance);
    }

    public override List<Node> VerNodosAlAlcance()
    {
        NodosAlAlcance = Control.GetNodosAlAlcance(m_Unidad.Nodo, Alcance);
        for (int i = NodosAlAlcance.Count - 1; i >= 0; i--)
        {
            if (NodosAlAlcance[i].unidad != null)
            {
                NodosAlAlcance.Remove(NodosAlAlcance[i]);
            }
        }
        return NodosAlAlcance;
    }
}
