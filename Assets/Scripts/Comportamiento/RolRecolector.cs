using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RolRecolector : MonoBehaviour {

	public enum EstadoRecoleccion
	{
		BajoDemanda, EnCamino, DemandaSatisfecha
	};
		
	private Partida partidaActual;
	private List<IA_Aldeano> aldeanos;

	public const int MINIMO_ALDEANOS_DISPONIBLES = 2;

	// EL COSTE DE COMIDA ES TAMBIEN EL DE BASE, PARA HACER PRUEBAS.
	public const int MINIMO_COMIDA = 40;
	public const int MINIMO_MADERA = 40;
	public const int MINIMO_METAL = 40;
	public const int MINIMO_PIEDRA = 40;

	//AÑADE UNA FUNCION QUE RECOJA DE TODAS LAS SOURCES DE RECURSOS DISPONIBLES.


	//RECUERDA: ESTE SCRIPTE DEBE IR EN EL MISMO GAMEOBJECT QUE EL JUGADOR.
	void Start()
	{
		aldeanos = new List<IA_Aldeano> ();
		partidaActual = StageData.currentInstance.GetPartidaActual ();
	}

	/*public bool ComenzarTurno(int puntosAsignados, int recursoBase)
	{
		// Buscamos ahora a los aldeanos disponibles.
		aldeanos.Clear();
		for (int i = 0; i < partidaActual.JugadorActual.unidadesDisponibles.Count; i++) 
		{
			if (partidaActual.JugadorActual.unidadesDisponibles [i].IdUnidad == TipoUnidad.Worker) 
			{
				aldeanos.Add ((IA_Aldeano)partidaActual.JugadorActual.unidadesDisponibles [i]);
			}
		}

		//Comprobamos si necesitamos recursos mínimos, para cumplir necesidades mínimas.
		if (!CheckMinimumPlaceHolder (recursoBase)) 
		{
			//Necesitamos recursos.

			if (aldeanos.Count < MINIMO_ALDEANOS_DISPONIBLES) 
			{
				//No tenemos tampoco aldeanos suficientes. 
				//Por ahora vamos a spawnear 2 aldeanos.
				//Debemos buscar ahora el lugar más cercano para crear los aldeanos.
				// Por ahora, 

			}

		}
	}
	*/

	public bool CheckMinimumResources(FacturaRecursos recActuales)
	{
		return recActuales.GetComida () >= MINIMO_COMIDA &&
		recActuales.GetMadera () >= MINIMO_MADERA &&
		recActuales.GetMetal () >= MINIMO_METAL &&
		recActuales.GetPiedra () >= MINIMO_PIEDRA;
	}

	public bool CheckMinimumPlaceHolder (int recursoBase)
	{
		return recursoBase >= MINIMO_COMIDA;
	}

	//NECESITAMOS VARIAS FUNCIONES: 
	/*
	 *1 - crear aldeanos,
	 *2 - mandar a los aldeanos a recoger los recursos.
	 *3 - funcion para obtener a los posibles creadores de aldeanos.
	 *4 - Funcion para obtener la influencia de los recursos, de ser encontrados.
	 *5 - funcion para obtener los diferentes recursos de la partida.

	*/

	/*
	void CrearAldeanosParaRecurso()
	{
		//Obtenemos todos los posibles, del jugador que queremos. 
		List<Unidad> edificiosCreadoresDeAldeanos = partidaActual.JugadorActual.edificios;
		for (int i = edificiosCreadoresDeAldeanos.Count - 1; i >= 0; i++) 
		{
			if (edificiosCreadoresDeAldeanos [i].IdUnidad != TipoUnidad.Building) 
			{
				edificiosCreadoresDeAldeanos.Remove (edificiosCreadoresDeAldeanos [i]);
			}
		}
		edificiosCreadoresDeAldeanos.Add (partidaActual.JugadorActual.Capital);

		//Aqui, comprobar qué edificio está más cerca de qué recursos.



	}
	*/



}
