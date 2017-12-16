using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RolExplorador : MonoBehaviour {

    const int CREAR_UNIDAD_INDEX = 0;

	Partida partidaActual;
    bool exploradoresSuficientes, coroutineActive;

	void Start()
	{
		partidaActual = StageData.currentInstance.GetPartidaActual ();
	}

	public bool ComenzarTurno(int puntosAsignados)
	{
        exploradoresSuficientes = partidaActual.JugadorActual.Exploradores < 3;
        if (!exploradoresSuficientes)
            StartCoroutine("CrearExplorador");
        else
            MoverExploradores();
		//Decidir cual mover y cuanto moverlo

        return false;
	}

    private void MoverExploradores()
    {
        
    }

    IEnumerator CrearExplorador()
	{
        coroutineActive = true;
        List<Unidad> edificiosCreadores = GetCreadorDeUnidadesAdecuado(); // MIRO TODOS LOS EDIFICIOS QUE PUEDEN CONSTRUIR UNIDADES Y LOS ORDENO POR PRIORIDAD
        int edificioActual = 0; // SE UTILIZA PARA SABER CUAL ES EL EDIFICIO QUE VA A CONSTRUIR, PARA CONTROLAR QUE SI UN EDIFICIO NO PUEDE CREAR MAS, PASE AL SIGUIENTE
        while (partidaActual.JugadorActual.Exploradores < 3 && edificioActual < edificiosCreadores.Count)
        {
            CrearUnidad accionCreadorUnidades = (CrearUnidad)edificiosCreadores[edificioActual].Acciones[CREAR_UNIDAD_INDEX];
            List<Node> nodosAlAlcance = GetComponent<CrearUnidad>().VerNodosAlAlcance(); // COJO LOS NODOS AL ALCANCE ACTUALIZADO DEL EDIFICIO DE CREACION
            if (nodosAlAlcance.Count == 0)
            { // SI EN ESTE EDIFICIO NO SE PUEDE CREAR MAS GUERREROS, PASO AL SIGUIENTE
                edificioActual++;
                if (edificioActual >= edificiosCreadores.Count) // SI HE RECORRIDO TODOS LOS EDIFICIOS, SIGNIFICA QUE ME QUEDAN PUNTOS POR GASTAR PERO NO ME CABE EN NINGUN EDIFICIO MAS GUERREROS
                    print("NO SE PUEDEN CREAR MAS UNIDADES  PORQUE NO CABEN MAS UNIDADES");
            }
            else if (edificioActual < edificiosCreadores.Count) // EN CASO DE QUE TODO VAYA BIEN, LO CREO
            {
                bool haFuncionado = GetComponent<CrearUnidad>().Ejecutar(nodosAlAlcance[0], TipoUnidad.Warrior);
                if (haFuncionado)
                {
                    yield return new WaitForSeconds(0.5f); // DELAY QUE SE TARDA ENTRE CREAR UNA UNIDAD Y OTRA
                }
            }
        }
        exploradoresSuficientes = partidaActual.JugadorActual.Exploradores < 3;
        if(exploradoresSuficientes)
            MoverExploradores();
        coroutineActive = false;
    }

	List<Unidad> GetCreadorDeUnidadesAdecuado()
	{
		List<Unidad> edificiosCreadoresDeunidades = partidaActual.JugadorActual.edificios;
		for (int i = edificiosCreadoresDeunidades.Count - 1; i >= 0; i--) {
			if (edificiosCreadoresDeunidades [i].IdUnidad != TipoUnidad.Resource)
				edificiosCreadoresDeunidades.Remove (edificiosCreadoresDeunidades [i]);
		}
		edificiosCreadoresDeunidades.Add (partidaActual.JugadorActual.Capital);
        List<Unidad> edificiosOrdenadosPorPrioridad = new List<Unidad>(); // LA LISTA EN LA QUE VOY A METER LOS EDIFICIOS ORDENADOS POR PRIORIDAD
        Node objetivo = GetObjetivoExplorador(); // MIRO CUAL ES EL OBJETIVO
        Unidad aux;
        while (edificiosCreadoresDeunidades.Count > 0) // ORDENACION
        {
            aux = edificiosCreadoresDeunidades[0];
            foreach (Unidad u in edificiosCreadoresDeunidades)  // LOS ORDENO POR DISTANCIA. CUANTA MENOR ES LA DISTANCIA, MAYOR ES LA PRIORIDAD
            {
                if (Vector3.Distance(u.transform.position, objetivo.position) < Vector3.Distance(aux.transform.position, objetivo.position))
                    aux = u;
            }
            edificiosOrdenadosPorPrioridad.Add(aux);
            edificiosCreadoresDeunidades.Remove(aux);
        }
        return edificiosOrdenadosPorPrioridad;
    }

    private Node GetObjetivoExplorador()
    {
        Node[,] grafo = StageData.currentInstance.grafoTotal;
        List<Node> nodosSinVisitar = new List<Node>();
        for (int i = 0; i < grafo.GetLength(0); i++)
        {
            for (int j = 0; j < grafo.GetLength(1); j++)
            {
                if (grafo[i, j].GetPlayerInfluence(partidaActual.JugadorActual.idJugador) == -1)
                    nodosSinVisitar.Add(grafo[i,j]);
            }
        }
        Vector3 posCapital = partidaActual.JugadorActual.Capital.transform.position;
        Node aux = nodosSinVisitar[0];
        foreach (Node n in nodosSinVisitar)
        {
            if (Vector3.Distance(posCapital, n.position) < Vector3.Distance(posCapital, aux.position))
                aux = n;
                
        }
        return aux;
    }
}
