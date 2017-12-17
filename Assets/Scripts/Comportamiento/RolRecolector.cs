using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RolRecolector : MonoBehaviour {

	public enum EstadoRecoleccion
	{
		BajoDemanda, EnCamino, DemandaSatisfecha
	};

	private Partida partidaActual;
	List<Vector3> caminoARecursoDestino;
	bool fin;
	int puntosAsignados;
	int numeroCreaciones;
	List<Vector3> recursosSinExplotar;

	bool numeroAldeanosMINIMOS;

	TipoRecurso recursoMinimoActual;

	// EL COSTE DE COMIDA ES TAMBIEN EL DE BASE, PARA HACER PRUEBAS.
	public const int MINIMO_COMIDA = 40;
	public const int MINIMO_MADERA = 40;
	public const int MINIMO_METAL = 40;
	public const int MINIMO_PIEDRA = 40;

	//AÑADE UNA FUNCION QUE RECOJA DE TODAS LAS SOURCES DE RECURSOS DISPONIBLES.


	//RECUERDA: ESTE SCRIPTE DEBE IR EN EL MISMO GAMEOBJECT QUE EL JUGADOR.
	void Start()
	{
		partidaActual = StageData.currentInstance.GetPartidaActual ();
		caminoARecursoDestino = new List<Vector3> ();
		recursosSinExplotar = new List<Vector3> ();
	}

	public bool ComenzarTurno(int puntosAsig)
	{
		// Buscamos ahora a los aldeanos disponibles.
		puntosAsignados = puntosAsig;
		numeroAldeanosMINIMOS = partidaActual.JugadorActual.Aldeanos < 3;
		recursoMinimoActual = partidaActual.JugadorActual.GetMenorTipoRecurso ();
		recursosSinExplotar.Clear ();

		if (!numeroAldeanosMINIMOS)
		{
			numeroCreaciones = puntosAsignados / StageData.COSTE_PA_CREAR_ALDEANO;
			if (numeroCreaciones > 0) 
			{
				StartCoroutine ("CrearAldeano");
				return true;
			}
			else
			{
				return false;
			}
			//Empieza la corutina de crear los aldeanos.
		}
		/*if (partidaActual.JugadorActual.posicionRecursosEncontrados.Count < 3) 
		{
			//Mandamos a la peña a recoger recursos, para ello dedicaremos los recursos a Explorador.
		}*/
		else 
		{
			int movimientosDisponibles = puntosAsig / StageData.COSTE_PA_MOVER_UNIDAD;

			if (movimientosDisponibles > 0) 
			{
				MoverAldeanosDisponibles (movimientosDisponibles);
				return true;
			} 
			else
			{
				return false;
			}

		}

		//Comprobamos si necesitamos recursos mínimos, para cumplir necesidades mínimas.

	}

	private void MoverAldeanosDisponibles(int numeroMovimientos)
	{
		//AQUI MIRO SI TENGO ALDEANOS CERCA.
		List<Unidad> aldeanos = partidaActual.JugadorActual.unidadesDisponibles;
		for (int i = aldeanos.Count - 1; i >= 0; i--) 
		{
			if (aldeanos [i].IdUnidad == TipoUnidad.Worker) 
			{
				aldeanos.Remove (aldeanos[i]);
			}
		}
		List<Unidad> aldeanosOrdenadosPorProximidad = new List<Unidad> ();

		SetResourcesSinExplotar (recursoMinimoActual);

		Unidad aux = aldeanos[0];
		Vector3 cerca = recursosSinExplotar[0];

		while (aldeanos.Count > 0 && recursosSinExplotar.Count > 0) // ORDENACION
		{
			foreach (Unidad u in aldeanos)  // LOS ORDENO POR DISTANCIA. CUANTA MENOR ES LA DISTANCIA, MAYOR ES LA PRIORIDAD
			{
				foreach (Vector3 pos in recursosSinExplotar) //BUSCAMOS EL RECURSO MAS CERCANO RESPECTO LA CAPITAL.
				{
					if (Vector3.Distance (pos, u.Nodo.position) < Vector3.Distance (cerca, u.Nodo.position)) 
					{
						cerca = pos;
						aux = u;
					}
				}

			}
			recursosSinExplotar.Remove (cerca);
			aldeanosOrdenadosPorProximidad.Add(aux);
			aldeanos.Remove(aux);
		}






	}


	public bool CheckMinimumResources()
	{
		return partidaActual.JugadorActual.ComprobarRecursosNecesarios (
			new FacturaRecursos (0, MINIMO_COMIDA, MINIMO_MADERA, MINIMO_PIEDRA, MINIMO_METAL)); 
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

	IEnumerator CrearAldeano()
	{
		List<Unidad> edificiosCreadores = GetCreadorDeAldeanosAdecuado ();
		int edificioActual = 0;
		while (numeroCreaciones > 0 && edificioActual < edificiosCreadores.Count)
		{
			CrearUnidad accionCreadorUnidades = (CrearUnidad)edificiosCreadores [edificioActual].Acciones [0];
			List<Node> nodosAlAlcance = GetComponent<CrearUnidad> ().VerNodosAlAlcance ();
			if (nodosAlAlcance.Count == 0) 
			{
				edificioActual++;
				if (edificioActual >= edificiosCreadores.Count)
				{
					print ("No se pueden crear más en este edificio");
				}
			}
			else if (edificioActual < edificiosCreadores.Count)
			{
				bool haFuncionado = GetComponent<CrearUnidad>().Ejecutar(nodosAlAlcance[0], TipoUnidad.Worker);
				if (haFuncionado)
				{
					numeroCreaciones--;
					print (numeroCreaciones);
					yield return new WaitForSeconds(0.5F);
				}
			}
		}


	}

	List<Unidad> GetCreadorDeAldeanosAdecuado()
	{
		List<Unidad> edificiosCreadoresDeunidades = partidaActual.JugadorActual.edificios;
		for (int i = edificiosCreadoresDeunidades.Count - 1; i >= 0; i--) 
		{
			if (edificiosCreadoresDeunidades [i].IdUnidad == TipoUnidad.DefensiveBuilding) 
			{
				edificiosCreadoresDeunidades.Remove (edificiosCreadoresDeunidades [i]);
			}

		}
		edificiosCreadoresDeunidades.Add (partidaActual.JugadorActual.Capital);
		List<Unidad> edificiosOrdenadosPorPrioridad = new List<Unidad>(); // LA LISTA EN LA QUE VOY A METER LOS EDIFICIOS ORDENADOS POR PRIORIDAD

		//COMPROBAR EL RECURSO NECESARIO QUE ESTAMOS BUSCANDO.
		SetResourcesSinExplotar (recursoMinimoActual);

		Unidad aux;
		Vector3 cerca;
		while (edificiosCreadoresDeunidades.Count > 0 && recursosSinExplotar.Count > 0) // ORDENACION
		{
			aux = edificiosCreadoresDeunidades[0];
			cerca = recursosSinExplotar[0];
			foreach (Unidad u in edificiosCreadoresDeunidades)  // LOS ORDENO POR DISTANCIA. CUANTA MENOR ES LA DISTANCIA, MAYOR ES LA PRIORIDAD
			{

				foreach (Vector3 pos in recursosSinExplotar) //BUSCAMOS EL RECURSO MAS CERCANO RESPECTO LA CAPITAL.
				{
					if (Vector3.Distance (pos, u.Nodo.position) < Vector3.Distance (cerca, u.Nodo.position)) 
					{
						cerca = pos;
						aux = u;
					}
				}

			}
			recursosSinExplotar.Remove (cerca);
			edificiosOrdenadosPorPrioridad.Add(aux);
			edificiosCreadoresDeunidades.Remove(aux);
		}

		return edificiosOrdenadosPorPrioridad;

	}

	private void SetResourcesSinExplotar(TipoRecurso recursoDeseado)
	{
		Node[,] grafo = StageData.currentInstance.grafoTotal;
		List<Unidad> unidades_jugador = partidaActual.JugadorActual.unidadesDisponibles;
		List<Vector3> posRecursosSinExplorar = new List<Vector3> ();
		foreach (Unidad u in unidades_jugador)
		{
			//Aqui esta como quie muy harcodeado lol xd.
			List<Node> VisionDeNodos = Control.GetNodosAlAlcance (u.Nodo, 5);
			foreach (Node n in VisionDeNodos)
			{
				if (n.GetPlayerInfluence (partidaActual.JugadorActual.idJugador) != 1 &&
					n.resourceType == recursoDeseado && n.unidad != null) 
				{
					AddRecursoSinExplotar (n.position);
				}
			}
		}
	}


	private void AddRecursoSinExplotar(Vector3 pos)
	{
		if (!recursosSinExplotar.Contains (pos)) 
		{
			recursosSinExplotar.Add (pos);
		}
	}


}
