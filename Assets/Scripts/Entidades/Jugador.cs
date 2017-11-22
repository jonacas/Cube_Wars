using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jugador
{

    #region CONSTANTES
    private int ID_JUGADOR_HUMANO = 0;

    private int MADERA_INICIAL = 100;
    private int METAL_INICIAL = 20;
    private int PIEDRA_INICIAL = 40;
    private int COMIDA_INICIAL = 200;
    #endregion


    List<Unidad> unidadesDisponibles;

    Unidad capital = null; //unidad que hay que destruir para ganar la partida

    int puntosDeAccion;
    int idJugador;

    #region RECURSOS
    int madera;
    int metal;
    int piedra;
    int comida;
    #endregion

    public Jugador(int id, Unidad capital)
    {
        idJugador = id;

        if (id == ID_JUGADOR_HUMANO)
        {
            madera = MADERA_INICIAL * (GlobalData.MAX_DIFICULTAD - GlobalData.DIFICULTAD);
            metal = METAL_INICIAL * (GlobalData.MAX_DIFICULTAD - GlobalData.DIFICULTAD);
            piedra = PIEDRA_INICIAL * (GlobalData.MAX_DIFICULTAD - GlobalData.DIFICULTAD);
            comida = COMIDA_INICIAL * (GlobalData.MAX_DIFICULTAD + 1 - (GlobalData.DIFICULTAD)); //para evitar que sea 0
        }

        else
        {
            madera = MADERA_INICIAL * (GlobalData.DIFICULTAD);
            metal = METAL_INICIAL * (GlobalData.DIFICULTAD);
            piedra = PIEDRA_INICIAL * (GlobalData.DIFICULTAD);
            comida = COMIDA_INICIAL * (GlobalData.DIFICULTAD);
        }

        puntosDeAccion = 100;
        this.capital = capital;
    }


    public void DestruirUnidad(Unidad u)
    {
        if(!unidadesDisponibles.Remove(u))
            Debug.Log("Error al eliminr unidad en jugador " + idJugador);

    }

    public void AnadirUnidad(Unidad u)
    {
        unidadesDisponibles.Add(u);
    }

    public int SaludCapital()
    {
        return capital.Vida;
    }

}
