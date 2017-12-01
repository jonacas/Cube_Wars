using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Jugador
{

    #region CONSTANTES
    private int ID_JUGADOR_HUMANO = 0;

    private int MADERA_INICIAL = 100;
    private int METAL_INICIAL = 20;
    private int PIEDRA_INICIAL = 40;
    private int COMIDA_INICIAL = 200;
    #endregion


    List<Unidad> unidadesDisponibles;
    List<Unidad> edificios;

    Unidad capital = null; //unidad que hay que destruir para ganar la partida

    int puntosDeAccion;
    public int PuntosDeAccion
    {
        get { return puntosDeAccion; }
    }
    int idJugador;

    #region RECURSOS
    int madera;
    int metal;
    int piedra;
    int comida;
    #endregion

    bool activo = true; //indica si el jugador ha perdido o si sigue jugando

    //mapa influencias del jugador
    Node[] influencias;

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
        unidadesDisponibles = new List<Unidad>();
        edificios = new List<Unidad>();
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

    public void CapitalDestruida()
    {
        activo = false;
    }

    public bool GetActivo()
    {
        return activo;
    }

    public bool RestarPuntosDeAccion(int valor)
    {
        if ((puntosDeAccion - valor) < 0)
            return false;
        else
        {
            puntosDeAccion -= valor;
            return true;
        }
    }

    public void SumarRecursos(TipoRecurso idRecurso, int cantidad)
    {
        switch (idRecurso)
        {
            case TipoRecurso.Food:
                comida += cantidad;
                break;
            case TipoRecurso.Wood:
                madera += cantidad;
                break;
            case TipoRecurso.Steel:
                metal += cantidad;
                break;
            case TipoRecurso.Rock:
                piedra += cantidad;
                break;
        }
    }

    public abstract void Turno(ref bool turnoFinalizado);


    /// <summary>
    /// Comprueba si tiene los recursos suficientes
    /// </summary>
    /// <param name="fac">Los recursos necesarios</param>
    /// <returns>True si los tiene, false si no</returns>
    public bool ComprobarRecursosNecesarios(FacturaRecursos fac)
    {
        return puntosDeAccion >= fac.GetPA() &&
            madera >= fac.GetMadera() &&
            comida >= fac.GetComida() &&
            piedra >= fac.GetPiedra() &&
            metal >= fac.GetMetal();
    }

    /// <summary>
    /// Intenta restar los recursos que cuesta una accion
    /// </summary>
    /// <param name="fac">Costes</param>
    /// <returns>True si ha sido posibl, false si no</returns>
    public bool RestarRecursos(FacturaRecursos fac)
    {
        if (ComprobarRecursosNecesarios(fac))
        {
            puntosDeAccion -= fac.GetPA();
            comida -= fac.GetComida();
            madera -= fac.GetMadera();
            metal -= fac.GetMetal();
            piedra -= fac.GetPiedra();
            return true;
        }
        else
            return false;
    }

}
