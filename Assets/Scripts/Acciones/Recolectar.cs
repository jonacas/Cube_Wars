using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recolectar : Accion {

    Unidad m_Unidad;

    private void Awake()
    {
        m_Unidad = GetComponent<Unidad>();
    }

    public bool Ejecutar(int idRecurso, int cantidad)
    {
        Unidad unidadActual = GetComponent<Unidad>();
        Jugador jugador = Partida.GetPartidaActual().Jugadores[unidadActual.IdJugador];

        jugador.SumarRecursos(idRecurso, cantidad);

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

    public override void CompletarAccion()
    {
        //necesito la informacion del nodo objetivo para poder ejecutarla
    }
}
