using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Construir : Accion {

    private const float OFFSET_Y = 2F;

    public List<Node> alcance;

    Unidad m_Unidad;
    GameObject fantasmaTorre;
    GameObject fantasmaEdificioRecoleccion;

    void Awake()
    {
        //aqui se deben coger los fantasmas que se mostraran para no instanciarlos mas tarde
        costeAccion = 50;
        m_Unidad = GetComponent<Unidad>();
    }

    /// <summary>
    /// Esta funcion muestra una version transparente de lo que se va a construir sobre el nodo en el que se encuentra el raton
    /// solo si está dentro del alcance. Llamar a SetAlcance() antes.
    /// </summary>
    /// <param name="nodo">Nodo sobre el que esta el raton</param>
    public void MostrarFantasmas(Node nodo)
    {
        Debug.LogError("ERROR EN ACCION CONSTRUIR: La funcion aun no esta del todo implementada");
        if (alcance != null)
        {
            if (alcance.Contains(nodo))
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
    bool Ejecutar(Node n)
    {
        //si el nodo esta al alcance
        if (alcance.Contains(n))
        {
            if (n.unidad == null)
            {
                if (n.resourceType == null)
                {
                    if (Partida.GetPartidaActual().Jugadores[m_Unidad.IdJugador].RestarPuntosDeAccion(costeAccion))
                    {
                        //instanciamos torre de defensa
                        return true;
                    }
                }
                else
                {//por si queremos poner costes distintos
                    if (Partida.GetPartidaActual().Jugadores[m_Unidad.IdJugador].RestarPuntosDeAccion(costeAccion))
                    {
                        //instanciamos edificio de recoleccion
                        return true;
                    }
                }
            }
        }
        return false;
    }

    /// <summary>
    /// Esta funcion indica los nodos en los que la accion podria realizarse
    /// </summary>
    /// <param name="nodos"> Lista de nodos dentro dle rango de alcance de la accion</param>
    public void SetAlcance(List<Node> nodos)
    {
        alcance = nodos;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (AccionEmpezada &&
            StageData.currentInstance.LastClickedNode.unidad == null &&
            StageData.currentInstance.LastClickedNode.resourceType == StageData.ResourceType.NullResourceType
            /*&& COMPROBAR QUE ESTÁ AL ALCANCE*/)
            {
                Ejecutar(StageData.currentInstance.LastClickedNode);
            }
        }
    }

    public override void CancelarAccion()
    {
        fantasmaTorre.SetActive(false);
        fantasmaEdificioRecoleccion.SetActive(false);
        alcance = null;
        AccionEmpezada = true;
        //codigo para des-resaltar las casillas del alcance
    }

    public override void EmpezarAccion()
    {
        m_Unidad.ResaltarCasillasAlAlcance(Alcance);
        AccionEmpezada = true;
    }
}
