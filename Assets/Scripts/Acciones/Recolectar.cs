using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recolectar : Accion {

    Unidad m_Unidad;

    private void Awake()
    {
        m_Unidad = GetComponent<Unidad>();
        Alcance = 1;
    }

    bool Ejecutar(TipoRecurso tipo, int cantidad)
    {
        Unidad unidadActual = GetComponent<Unidad>();
        Jugador jugador = Partida.GetPartidaActual().Jugadores[unidadActual.IdJugador];

        jugador.SumarRecursos(tipo, cantidad);

        return true;        
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
