using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IA_Exploradores : MonoBehaviour {
	/*
	const int CREAR_UNIDAD_INDEX = 0;

	List<Explorador> exploradores;
	Partida partidaActual;

	void Awake()
	{
		partidaActual = StageData.currentInstance.GetPartidaActual ();
	}

	public bool ComenzarTurno(float prioridadExploracion)
	{
		if (partidaActual.JugadorActual.Exploradores < 3)
			CrearAldeano ();
		//Decidir cual mover y cuanto moverlo
        return false;
	}

	void CrearAldeano()
	{
		CrearUnidad creadorActual = (CrearUnidad) GetCreadorDeUnidadesAdecuado ().Acciones [CREAR_UNIDAD_INDEX];
		List<Node> nodosAlAlcance = creadorActual.GetNodosAlAlcance ();
		//Decidir cual es el mejor nodo
		creadorActual.Ejecutar(nodosAlAlcance[0], TipoUnidad.Explorer);
	}

	Unidad GetCreadorDeUnidadesAdecuado()
	{
		List<Unidad> edificiosCreadoresDeunidades = partidaActual.JugadorActual.EdificiosRecoleccion;
		edificiosCreadoresDeunidades.Add (partidaActual.JugadorActual.Capital);
		//Decidir cual es el mejor para crearlo
		return edificiosCreadoresDeunidades [0];
	}*/
}
