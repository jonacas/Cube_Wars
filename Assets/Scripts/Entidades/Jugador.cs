using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Jugador
{

    #region CONSTANTES
    private int ID_JUGADOR_HUMANO = 1;

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

	//==== PARTE SETTEO DE INFLUENCIAS ====
	/*
		PARTE POR HACER. DUDAS Y PROCEDIMIENTO A SEGUIR:
		PROBLEMAS Y DUDAS:
		1 - PARA SETTEAR LA INFLUENCIA DEL JUGADOR, DEBO PODER COGER SU TIPO. SIN EMBARGO,
		    NO ES POSIBLE DEBIDO A QUE SE GUARDAN SOLO LAS UNIDADES, NO LOS HIJOS DE ESTAS.
		2 - PARA SETTEAR LOS RECURSOS, MÁS DE LO MISMO.

		FUNCIONES A USAR:
		* RECUERDA QUE EN LA LISTA DE INFLUENCIAS, EL 0 ESTÁ RESERVADO PARA LOS RECURSOS, Y EL 1 PARA EL JUGADOR HUMANO.
		* SI QUEREMOS CAMBIAR LA CANTIDAD DE INFLUENCIA QUE TIENE, VE A NODE.CS Y VERAS UNAS VARIABLES PARA ELLO (STEPSINFLUENCE).
		* 
		* SI QUEREMOS SETTEAR LA INFLUENCIA DE UN TIPO DE UNIDAD QUE NO ES RECURSO: 	SETINFLUENCE(TIPOUNIDAD, INT JUGADOR);
		* SI QUEREMOS SETTEAR LA INFLUENCIA DE UN RECURSO: 								SETRESOURCEINFLUENCE(TIPORECURSO);
		* SI QUEREMOS OBTENER LA MÁXIMA INFLUENCIA DE UN JUGADOR EN UN NODO: 			GETMAXINFLUENCEFROMPLAYER(INT JUGADOR);
		* SI QUEREMOS OBTENER TODAS LAS INFLUENCIAS DEL JUGADOR EN UN NODO:				GETALLINFLUENCESFROMPLAYER(INT JUGADOR);
		* SI QUEREMOS LIMPIAR TODAS LAS INFLUENCAIS DEL JUGADOR EN UN NODO:				CLEARALLPLAYERINFLUENCES(INT JUGADOR);
		* SI QUEREMOS LIMPIAR LA INFLUENCIA DE UN JUGADOR, SEGUN LA UNIDAD DEL NODO:	CLEARINFLUENCE(TIPOUNIDAD, INT JUGADOR);
		* SI QUEREMOS LIMPIAR LA INFLUENCIA DE UN SOLO RECURSO EN EL NODO: 				CLEARSPECIFICRECOURSE(TIPORECURSO);
		* SI QUEREMOS LIMPIAR LA INFLUENCIA ESPECIFICA DE UN JUGADOR EN EL NODO:		CLEARSPECIFICINFLUENCEFROMPLAYER(INT JUGADOR, INT RESOURCE);
		* SI QUEREMOS LIMPIAR TODOS LOS RECURSOS DEL NODO: 								CLEARALLRECOURSEINFLUENCE();
		* SI QUEREMOS OBTENER LA INFLUENCIA DE LOS ENEMIGOS:	
		*   --> BUSCA CON GETMAXINFLUENCE A CADA JUGADOR QUE DESEES, QUE NO SEA EL PROPIO, O EL 0, SI QUIERES SOLO LA MAXIMA.
		*   --> DE QUERER TODAS, USA GETALLINFLUENCESFROMPLAYER(INT JUGADOR).

		IMPORTANTE:
		ANTES DE SETTEAR NADA, DEBEMOS AÑADIR EL NUMERO DE JUGADORES A LA PARTIDA USANDO 		ADDPLAYERTOINFLUENCES();
		ESTA LLAMADA SE ESTA HACIENDO ACTUALMENTE EN CREACIONGRAFO.CS



	*/
}
