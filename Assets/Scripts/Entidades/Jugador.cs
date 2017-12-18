using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Jugador : MonoBehaviour
{
    public List<Unidad> unidadesDisponibles;
    public List<Unidad> edificios;
    public Unidad[] capitalesEnemigas;

	public int IndexPlayerObjetivoActual;

    protected bool turnoAcabado;

	Unidad capital = null;
	public Unidad Capital {
		get {
			return capital;
		}
	}

	public List<Vector3> posicionRecursosEncontrados;

 //unidad que hay que destruir para ganar la partida

    protected int puntosDeAccion;
    public int PuntosDeAccion
    {
        get { return puntosDeAccion; }
    }
    public int idJugador;

    #region RECURSOS
    int madera;
    int metal;
    int piedra;
    int comida;
    #endregion


    #region UNIDADES
    private int exploradores;
    public int Exploradores
    {
        get { return exploradores; }
        set { exploradores = value; }
    }

    private int guerreros;
    public int Guerreros
    {
        get { return guerreros; }
        set { guerreros = value; }
    }

    private int aldeanos;
    public int Aldeanos
    {
        get { return aldeanos; }
        set { aldeanos = value; }
    }

    private int edificiosRecoleccion;
    public int EdificiosRecoleccion
    {
        get { return edificiosRecoleccion; }
        set { edificiosRecoleccion = value; }
    }

    private int torresDefensa;
    public int TorresDefensa
    {
        get { return torresDefensa; }
        set { torresDefensa = value; }
    }
    #endregion

    bool activo = true; //indica si el jugador ha perdido o si sigue jugando

    //mapa influencias del jugador
    public Node[,] influencias;

    public void Crearjugador(int id, Unidad capital)
    {
        idJugador = id;

        if (id == StageData.ID_JUGADOR_HUMANO)
        {
            madera = StageData.MADERA_INICIAL * (GlobalData.MAX_DIFICULTAD +1 - GlobalData.DIFICULTAD);
            metal = StageData.METAL_INICIAL * (GlobalData.MAX_DIFICULTAD +1 - GlobalData.DIFICULTAD);
            piedra = StageData.PIEDRA_INICIAL * (GlobalData.MAX_DIFICULTAD +1 - GlobalData.DIFICULTAD);
            comida = StageData.COMIDA_INICIAL * (GlobalData.MAX_DIFICULTAD + 1 - (GlobalData.DIFICULTAD)); //para evitar que sea 0
        }

        else
        {
            madera = StageData.MADERA_INICIAL * (GlobalData.DIFICULTAD);
            metal = StageData.METAL_INICIAL * (GlobalData.DIFICULTAD);
            piedra = StageData.PIEDRA_INICIAL * (GlobalData.DIFICULTAD);
            comida = StageData.COMIDA_INICIAL * (GlobalData.DIFICULTAD);
        }

        puntosDeAccion = 100;
        this.capital = capital;
        unidadesDisponibles = new List<Unidad>();
        edificios = new List<Unidad>();
		posicionRecursosEncontrados = new List<Vector3> ();

        //unidades
        exploradores = 0;
        guerreros = 0;
        aldeanos = 0;
        edificiosRecoleccion = 0;
        torresDefensa = 0;

		//Con esto copiamos un mapa de nodos limpio ^^
		influencias = StageData.currentInstance.CG.nodeMap;
    }


    public void DestruirUnidad(Unidad u)
    {
        if(!unidadesDisponibles.Remove(u))
            {
            if(!edificios.Remove(u))
                Debug.Log("Error al eliminr unidad en jugador " + idJugador);
        }

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

    public abstract void Turno();


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
		* AHORA, SE LLAMAN LAS FUNCIONES DESDE STAGEDATA, PASANDO COMO PARAMETROS EL IDJUGADOR, EL GRAFO DE ESTE, 
		* EL NUMERO DE DISPERSION DE INFLUENCIA, Y EL NODO CENTRAL DE LA DISPERSION.
		* PARA BORRAR, SE PASAN LOS MISMOS PARÁMETROS, RESETEANDO LOS VALORES DE CADA JUGADOR.
		* PARA LOS RECURSOS, SE DEBE HACER QUE EL IDJUGAOR == 0.

		IMPORTANTE:
		ANTES DE SETTEAR NADA, DEBEMOS AÑADIR EL NUMERO DE JUGADORES A LA PARTIDA USANDO 		ADDPLAYERTOINFLUENCES();
		ESTA LLAMADA SE ESTA HACIENDO ACTUALMENTE EN CREACIONGRAFO.CS
	*/

    public void TerminarTurno()
    {
        turnoAcabado = true;
    }

    public bool HaAcabadoTurno()
    {
        return turnoAcabado;
    }

	public void RecursoEncontrado(Vector3 pos)
	{
		if (!posicionRecursosEncontrados.Contains (pos)) 
		{
            //print("Nuevo recurso encontrado");
			posicionRecursosEncontrados.Add (pos);
		}
	}

	public TipoRecurso GetMenorTipoRecurso()
	{
		if (piedra < comida && piedra < madera && piedra < metal) 
		{
			return TipoRecurso.Rock;
		}
		else if (madera < comida && madera < piedra && madera < metal) 
		{
			return TipoRecurso.Wood;
		}
		else if (metal < comida && metal < madera && metal < piedra) 
		{
			return TipoRecurso.Steel;
		}
		else
		{
			return TipoRecurso.Food;
		}
	}



}
