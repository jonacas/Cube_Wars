using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Atacar : Accion {

    Unidad m_Unidad;

    private void Awake()
    {
        m_Unidad = GetComponent<Unidad>();
    }

    public bool Ejecutar(GameObject objetivo)
    {
        
        if (Partida.GetPartidaActual().Jugadores[m_Unidad.IdJugador].RestarPuntosDeAccion(costeAccion))
        {
            //Hay que controlar si el objetivo está al alcance de la unidad que ataca cuando se llama a esta funcion.
            try
            {
                Unidad atacante = gameObject.GetComponent<Unidad>();
                Unidad victima = objetivo.GetComponent<Unidad>();

                victima.RecibirAtaque(m_Unidad.Danyo);

                atacante.RecibirAtaque(victima.DanyoContraataque);

                //Ejecutar alguna animacion en caso de que se hiciera, para ver que se está atacando y no que haya solo dos cubos quietos.
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError("Se está intentando atacar a algo sin el componente Unidad");
                return false;
            }
        }
        return false;
    }    

    public override void CancelarAccion()
    {
        //codigo para des-resaltar las casillas del alcance
    }

    public override void EmpezarAccion()
    {
        m_Unidad.ResaltarCasillasAlAlcance(Alcance);
    }

    public override void CompletarAccion()
    {
        //necesito la informacion del nodo objetivo para poder ejecutarla
    }
}
