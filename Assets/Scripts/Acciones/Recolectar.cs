using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recolectar : Accion {

    public bool Ejecutar(int idRecurso, int cantidad)
    {
        Unidad unidadActual = GetComponent<Unidad>();
        Jugador jugador = Partida.GetPartidaActual().Jugadores[unidadActual.IdJugador];

        jugador.SumarRecursos(idRecurso, cantidad);

        return true;        
    }

}
