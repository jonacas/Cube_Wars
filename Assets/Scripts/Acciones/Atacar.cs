using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Atacar : Accion {

    public List<Node> NodosAlAlcance;
    
    private void Awake()
    {
        idAccion = AccionID.attack;
        m_Unidad = GetComponent<Unidad>();
        switch (m_Unidad.IdUnidad)
        {
            case TipoUnidad.Warrior: //en caso de que al final se añadan otras unidades, pues ya sabes loko
                Alcance = 2;
                break;
            case TipoUnidad.DefensiveBuilding:
                Alcance = 4;
                break;
        }
    }

    public bool Ejecutar(Node victima)
    {
        print("entra");
        SeleccionarResaltoDeCasilla();
        if (NodosAlAlcance.Contains(victima)) {
            if (Partida.GetPartidaActual().Jugadores[m_Unidad.IdJugador].PuntosDeAccion - costeAccion >= 0)
            {
                //Hay que controlar si el objetivo está al alcance de la unidad que ataca cuando se llama a esta funcion.
                try
                {
                    Unidad atacante = gameObject.GetComponent<Unidad>();

                    victima.unidad.RecibirAtaque(m_Unidad.Danyo);

                    atacante.RecibirAtaque(victima.unidad.DanyoContraataque);

                    Partida.GetPartidaActual().Jugadores[m_Unidad.IdJugador].RestarPuntosDeAccion(costeAccion);

                    //Ejecutar alguna animacion en caso de que se hiciera, para ver que se está atacando y no que haya solo dos cubos quietos.
                    //Des-resaltar casillas

                    CancelarAccion();

                    return true;
                }
                catch (Exception e)
                {
                    CancelarAccion();
                    Debug.LogError("Se está intentando atacar a algo sin el componente Unidad");
                    return false;
                }
            }
        }
        CancelarAccion();
        return false;
    }

    public override void CancelarAccion()
    {
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
        for(int i = NodosAlAlcance.Count-1; i >= 0; i--)
        {
            if (NodosAlAlcance[i].unidad == null ||
                NodosAlAlcance[i].unidad.IdJugador == m_Unidad.IdJugador)
            {
                NodosAlAlcance.Remove(NodosAlAlcance[i]);
            }
        }
    }
}
