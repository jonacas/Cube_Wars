using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RolLatente : MonoBehaviour {

	const int CREAR_UNIDAD_INDEX = 0;

	Partida partidaActual;
	int numeroCreaciones;

	void Awake()
	{
		partidaActual = StageData.currentInstance.GetPartidaActual ();
	}

	public bool ComenzarTurno(int puntosAsignados)
	{
		//Decidir cual mover y cuanto moverlo
		numeroCreaciones = puntosAsignados / StageData.COSTE_PA_CREAR_GUERRERO;
		if (numeroCreaciones > 0) {
			CrearGuerreros ();
			return true;
		}
		else
			return false;
	}

	void CrearGuerreros()
	{
		List<Unidad> edificiosCreadores = GetCreadorDeUnidadesAdecuado();
		int edificioActual = 0;
		while(numeroCreaciones > 0 && edificioActual < edificiosCreadores.Count)
		{
			CrearUnidad accionCreadorUnidades = (CrearUnidad) edificiosCreadores [edificioActual].Acciones [CREAR_UNIDAD_INDEX];
			List<Node> nodosAlAlcance = accionCreadorUnidades.GetNodosAlAlcance ();
			if(nodosAlAlcance.Count == 0){
				edificioActual++;
				if (edificioActual >= edificiosCreadores.Count)
					print ("NO SE PUEDEN CREAR MAS UNIDADES  PORQUE NO CABEN MAS UNIDADES");				
			}
			else if(edificioActual < edificiosCreadores.Count)
			{
				accionCreadorUnidades.Ejecutar (nodosAlAlcance [0], TipoUnidad.Warrior);
				numeroCreaciones--;
			}
		}
	}

	List<Unidad> GetCreadorDeUnidadesAdecuado()
	{
		List<Unidad> edificiosCreadoresDeunidades = partidaActual.JugadorActual.edificios;
		for (int i = edificiosCreadoresDeunidades.Count - 1; i >= 0; i--) {
			if (edificiosCreadoresDeunidades [i].IdUnidad != TipoUnidad.Resource)
				edificiosCreadoresDeunidades.Remove (edificiosCreadoresDeunidades [i]);
		}
		edificiosCreadoresDeunidades.Add (partidaActual.JugadorActual.Capital);
		List<Unidad> edificiosOrdenadosPorPrioridad = new List<Unidad>();
		Unidad objetivo = partidaActual.Jugadores[partidaActual.JugadorActual.IndexPlayerObjetivoActual].Capital;
		Unidad aux;
		while(edificiosCreadoresDeunidades.Count > 0)
		{
			aux = edificiosCreadoresDeunidades [0];
			foreach(Unidad u in edificiosCreadoresDeunidades)
			{
				if (Vector3.Distance (u.transform.position, objetivo.transform.position) < Vector3.Distance (aux.transform.position, objetivo.transform.position))
					aux = u;
			}
			edificiosOrdenadosPorPrioridad.Add (aux);
			edificiosCreadoresDeunidades.Remove (aux);
		}

		return edificiosCreadoresDeunidades;
	}

}
