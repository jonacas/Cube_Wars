using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Partida : MonoBehaviour{

    public int numJugadores;

    private static Partida partidaActual;

    int turnos; //turnos totales de la partida
    public int GetTurnos()
    {
        return turnos;
    }
    int jugadorActual; //jugador que esta jugando ahora
    Jugador ganador;

    Jugador[] jugadores;

    public Jugador[] Jugadores { get { return jugadores; } }

    void Awake()
    {
        partidaActual = this;
        empezarPartida();
    }


    void empezarPartida()
    {
        //cargarEscenario()
        crearJugadores();

        //comenzarBuclePrincipal
        StartCoroutine("BucleDeJuego");
    }

    private void crearJugadores()
    {
        jugadores = new Jugador[numJugadores];
        for (int i = 0; i < numJugadores; i++)
        {
            //FALTA CODIGO DE CREAR CAPITAL

            //se instancia un gaeobject Capital
            if (i == 0)
            {
            }
            //se crea el peronaje controlado por el jugador
            else
            {
            }
            //se crean los personajes controlados por la IA

        }
    }

    IEnumerator BucleDeJuego()
    {
        bool turnofinalizado = false;

        for (int i = 0; i < numJugadores; i++)
        {
            jugadores[i].Turno(ref turnofinalizado);

            while(!turnofinalizado)
                yield return null;
        }

    }

    public bool DescativarJugadorYComprobarVictoria(int jugador)
    {
        //desactivamos jugador
        jugadores[jugador].CapitalDestruida();
        //comprobamos victoria
        bool jugadorDetectado = false;
        foreach (Jugador j in jugadores)
        {
            //si hay un jugador activo
            if (j.GetActivo())
            {
                ganador = j;
                //y no habia detectado otro antes, indica que ya ha detectado un jugador
                if (!jugadorDetectado)
                    jugadorDetectado = true;
                //si ya habia detectado un jugador antes, no hay victoria
                else
                {
                    ganador = null;
                    return false;
                }
            }
        }        
        return true;
    }

    public static Partida GetPartidaActual()
    {
        return partidaActual;

    }

}
