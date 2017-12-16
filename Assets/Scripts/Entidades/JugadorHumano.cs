using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JugadorHumano : Jugador {

    /* void CrearJugadorHumano(int id, Unidad capital)
    {
        Crearjugador(id, capital);
    }*/



    public override void Turno()
    {
        turnoAcabado = false;
    }


    public void TerminarTurno()
    {
        turnoAcabado = true;
    }
}
