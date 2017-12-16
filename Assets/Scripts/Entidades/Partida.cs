﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Partida : MonoBehaviour{

    public int numJugadores;


    int turnos; //turnos totales de la partida
    public int GetTurnos()
    {
        return turnos;
    }
    Jugador jugadorActual; //jugador que esta jugando ahora

    public Jugador JugadorActual
    {
        get
        {
            return jugadorActual;
        }
    }

    Jugador ganador;

    Jugador[] jugadores;

    public Jugador[] Jugadores { get { return jugadores; } }

    

    void Start()
    {
        StageData.currentInstance.SetPartidaActual(this);
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
        //se cogen las capitales
        List<Unidad> capis = new List<Unidad>();
        foreach (Transform child in GameObject.Find("Capitales").transform)
        {
            capis.Add(child.GetComponent<Unidad>());
            child.gameObject.SetActive(false);
        }


        jugadores = new Jugador[numJugadores];
        for (int i = 0; i < numJugadores; i++)
        {
            //se crea el peronaje controlado por el jugador
            if (i == 0)
            {
                jugadores[i] = GameObject.Find("Jugador0").GetComponent<JugadorHumano>();
            }
            //se crean los personajes controlados por la IA
            else
                jugadores[i] = GameObject.Find("Jugador" + i).GetComponent<JugadorIA>();

            jugadores[i].Crearjugador(i, capis[i]);

            //activamos capitales
            capis[i].gameObject.SetActive(true);
            capis[i].IdJugador = i;
        }
    }

    IEnumerator BucleDeJuego()
    {

        for (int i = 0; i < numJugadores; i++)
        {
            jugadores[i].Turno();

            while(!jugadores[i].HaAcabadoTurno())
                yield return null;

            print("TURNO TERMINADO");
        }

        yield return null;

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

}
