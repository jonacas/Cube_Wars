using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RolExplorador : MonoBehaviour {
	
	const int CREAR_UNIDAD_INDEX = 0;

	List<Explorador> exploradores;
	Partida partidaActual;

	void Start()
	{
		partidaActual = StageData.currentInstance.GetPartidaActual ();
	}

	public bool ComenzarTurno(int puntosAsignados)
	{
		if (partidaActual.JugadorActual.Exploradores < 3)
			CrearExplorador ();
		//Decidir cual mover y cuanto moverlo

        return false;
	}

	void CrearExplorador()
	{
		CrearUnidad creadorActual = (CrearUnidad) GetCreadorDeUnidadesAdecuado ().Acciones [CREAR_UNIDAD_INDEX];
		List<Node> nodosAlAlcance = creadorActual.VerNodosAlAlcance ();
		//Decidir cual es el mejor nodo
		creadorActual.Ejecutar(nodosAlAlcance[0], TipoUnidad.Explorer);
	}

	Unidad GetCreadorDeUnidadesAdecuado()
	{
		List<Unidad> edificiosCreadoresDeunidades = partidaActual.JugadorActual.edificios;
		for (int i = edificiosCreadoresDeunidades.Count - 1; i >= 0; i--) {
			if (edificiosCreadoresDeunidades [i].IdUnidad != TipoUnidad.Resource)
				edificiosCreadoresDeunidades.Remove (edificiosCreadoresDeunidades [i]);
		}
		edificiosCreadoresDeunidades.Add (partidaActual.JugadorActual.Capital);
		//Decidir cual es el mejor para crearlo
		return edificiosCreadoresDeunidades [0];
	}
}
