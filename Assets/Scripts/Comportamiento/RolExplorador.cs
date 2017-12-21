using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RolExplorador : MonoBehaviour {

    const int CREAR_UNIDAD_INDEX = 0, NUMERO_MINIMO_EXPLORADORES = 3;

	Partida partidaActual;
    public bool fin;
    int puntosAsignado;


	public bool ComenzarTurno(int puntosAsignadosDesdeFuera)
	{
        fin = false;
        partidaActual = StageData.currentInstance.GetPartidaActual();
        //print("Comenza rol explorador");
        puntosAsignado = puntosAsignadosDesdeFuera;
        if (partidaActual.JugadorActual.Exploradores < NUMERO_MINIMO_EXPLORADORES)
            StartCoroutine("CrearExplorador");

        StartCoroutine("MoverExploradores");
		//Decidir cual mover y cuanto moverlo

        return false;
	}

    IEnumerator MoverExploradores()
    {
        //yield return new WaitForSeconds(1);
        //print("Comienza mover exploradores");
        Node objetivo;
        List<Unidad> aux1 = StageData.currentInstance.GetPartidaActual().JugadorActual.unidadesDisponibles;
        List<Unidad> exploradores = new List<Unidad>();
        foreach (Unidad un in aux1)
        {
            if (un.IdUnidad == TipoUnidad.Explorer)
                exploradores.Add(un);
        }

        IA_Explorador aux;
        for (int i = exploradores.Count - 1; i >= 0; i--)
        {
            if (exploradores[i].IdUnidad != TipoUnidad.Explorer)
                exploradores.Remove(exploradores[i]);
            aux = (IA_Explorador)exploradores[i];

            if (!aux.HaLlegado())
            {
                objetivo = GetObjetivoExplorador();
                aux.SetDestino(objetivo);
            }
        }

        while (puntosAsignado >= StageData.COSTE_PA_MOVER_UNIDAD)
        {
            foreach (IA_Explorador expl in exploradores)
            {
                //puntosAsignado -= StageData.COSTE_PA_MOVER_UNIDAD;
                expl.AvanzarHaciaDestino(ref puntosAsignado);
                yield return new WaitForSeconds(2);
                while (!expl.listo)
                    yield return null;
                if (puntosAsignado < StageData.COSTE_PA_MOVER_UNIDAD)
                    break;
                yield return null;
            }
            yield return null;
        }
        fin = true;
        
    }

    IEnumerator CrearExplorador()
	{
        //print("Comienza crear Exploradores");
        List<Unidad> edificiosCreadores = GetCreadorDeUnidadesAdecuado(); // MIRO TODOS LOS EDIFICIOS QUE PUEDEN CONSTRUIR UNIDADES Y LOS ORDENO POR PRIORIDAD
        int edificioActual = 0; // SE UTILIZA PARA SABER CUAL ES EL EDIFICIO QUE VA A CONSTRUIR, PARA CONTROLAR QUE SI UN EDIFICIO NO PUEDE CREAR MAS, PASE AL SIGUIENTE
        while (partidaActual.JugadorActual.Exploradores < NUMERO_MINIMO_EXPLORADORES && edificioActual < edificiosCreadores.Count)
        {
            CrearUnidad accionCreadorUnidades = (CrearUnidad)edificiosCreadores[edificioActual].Acciones[CREAR_UNIDAD_INDEX];
            List<Node> nodosAlAlcance = accionCreadorUnidades.VerNodosAlAlcance(); // COJO LOS NODOS AL ALCANCE ACTUALIZADO DEL EDIFICIO DE CREACION
            if (nodosAlAlcance.Count == 0)
            { // SI EN ESTE EDIFICIO NO SE PUEDE CREAR MAS GUERREROS, PASO AL SIGUIENTE
                edificioActual++;
                if (edificioActual >= edificiosCreadores.Count) // SI HE RECORRIDO TODOS LOS EDIFICIOS, SIGNIFICA QUE ME QUEDAN PUNTOS POR GASTAR PERO NO ME CABE EN NINGUN EDIFICIO MAS GUERREROS
                   print("NO SE PUEDEN CREAR MAS UNIDADES  PORQUE NO CABEN MAS UNIDADES");
            }
           else if (edificioActual < edificiosCreadores.Count) // EN CASO DE QUE TODO VAYA BIEN, LO CREO
            {
                bool haFuncionado = accionCreadorUnidades.Ejecutar(nodosAlAlcance[0], TipoUnidad.Explorer);
                if (haFuncionado)
                {
                    //print(partidaActual.JugadorActual.Exploradores);
                    puntosAsignado -= StageData.COSTE_PA_CREAR_ALDEANO;
                    yield return new WaitForSeconds(0.5f); // DELAY QUE SE TARDA ENTRE CREAR UNA UNIDAD Y OTRA
                }
            }
        }
        //yield return new WaitForSeconds(2);
        StartCoroutine("MoverExploradores");
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
        Node destino = null;
        Node aux;
        List<Node> alAlcance;
        int valor = 0;

        while (destino == null)
        {
            aux = grafo[UnityEngine.Random.Range(0, StageData.currentInstance.CG.filas), UnityEngine.Random.Range(0, StageData.currentInstance.CG.columnas)];
            alAlcance = Control.GetNodosAlAlcance(aux, 5);

            foreach(Node n in alAlcance)
            {
                valor += n.GetPlayerInfluence(StageData.currentInstance.GetPartidaActual().JugadorActual.idJugador);
            }

            if (valor < 20)
            {
                destino = aux;
            }

            else
            {
                valor = 0;
            }

        }

        return destino;
        /*Node[,] grafo = StageData.currentInstance.grafoTotal;
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

        print(aux.position);
        return grafo[5,5];*/
    }
}
