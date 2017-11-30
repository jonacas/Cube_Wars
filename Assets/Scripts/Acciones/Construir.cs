using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Construir : Accion {

    private const float OFFSET_Y = 2F;

    public List<Node> alcance;


    GameObject fantasmaTorre;
    GameObject fantasmaEdificioRecoleccion;

    void Awake()
    {
        //aqui se deben coger los fantasmas que se mostraran para no instanciarlos mas tarde
        costeAccion = 50;
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
                //comprobamos que en el nodo no hay una unidad enemiga
                    //si no hay un recurso, se muestra la torre
                        fantasmaTorre.transform.position = nodo.position + new Vector3(0, OFFSET_Y, 0);
                    //si hay un recurso, se muestra el edificio de recoleccion
                        fantasmaEdificioRecoleccion.transform.position = nodo.position + new Vector3(0, OFFSET_Y, 0);
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
    public bool Ejecutar(Jugador j, Node n)
    {
        if (j.RestarPuntosDeAccion(costeAccion))
        {
            //comprobar que en la casilla no haya edificios ni unidades enemigas

                //comprobamos si hay recurso
                    //instanciamos edificio recolectos y anadimos a la lista del jugador
                //si no lo hay
                    //instanciamos torre y anadimos a la lista del jugador
            return true;
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

    public override void CancelarAccion()
    {
        fantasmaTorre.SetActive(false);
        fantasmaEdificioRecoleccion.SetActive(false);
        alcance = null;
    }
}
