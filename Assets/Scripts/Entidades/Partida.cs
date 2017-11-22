using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Partida : MonoBehaviour {

    public int numJugadores;


    int turnos; //turnos totales de la partida
    int jugadorActual; //jugador que esta jugando ahora

    Jugador[] jugadores;

    void Awake()
    {


    }


    void EmpezarPartida()
    {
        crearJugadores();

    }

    private void crearJugadores()
    {
        jugadores = new Jugador[numJugadores];
        for (int i = 0; i < numJugadores; i++)
        {
            //FALTA CODIGO DE CREAR CAPITAL

           // jugadores[i] = new Jugador(i

        }
    }

}
