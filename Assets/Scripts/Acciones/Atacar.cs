using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Atacar : Accion {

    Unidad m_Unidad;
    
    private void Awake()
    {
        m_Unidad = GetComponent<Unidad>();
        switch (m_Unidad.IdUnidad)
        {
            case TipoUnidad.Warrior: //en caso de que al final se añadan otras unidades, pues ya sabes loko
                Alcance = 1;
                break;
            case TipoUnidad.DefensiveBuilding:
                Alcance = 4;
                break;
        }
    }

    bool Ejecutar(Unidad victima)
    {
        
        if (Partida.GetPartidaActual().Jugadores[m_Unidad.IdJugador].PuntosDeAccion - costeAccion >= 0)
        {
            //Hay que controlar si el objetivo está al alcance de la unidad que ataca cuando se llama a esta funcion.
            try
            {
                Unidad atacante = gameObject.GetComponent<Unidad>();

                victima.RecibirAtaque(m_Unidad.Danyo);

                atacante.RecibirAtaque(victima.DanyoContraataque);

                Partida.GetPartidaActual().Jugadores[m_Unidad.IdJugador].RestarPuntosDeAccion(costeAccion);

                //Ejecutar alguna animacion en caso de que se hiciera, para ver que se está atacando y no que haya solo dos cubos quietos.
                //Des-resaltar casillas
                
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
}
