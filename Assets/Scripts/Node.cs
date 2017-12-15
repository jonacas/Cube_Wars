using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Priority_Queue;

[System.Serializable]
public struct Pareja
{
    public Node nodo;
    public float distancia;

    public Pareja(Node nodo, float distancia)
    {
        this.nodo = nodo;
        this.distancia = distancia;
    }
}

public class Node
{
    public const int NO_ESTA_EN_LISTA_ABIERTOS = -1, EN_LISTA_CERRADOS = -2;

    public Vector3 position;
    public List<Pareja> arrayVecinos;
    private int queuePosition;
    private float estimated;
    private Node route;
    private float cost;
	private bool water;
    public int fil, col;

	public List<List<int>> influencePlayers;
	public List<TipoRecurso> currentResources;

	public Unidad unidad;
	public TipoRecurso resourceType;
	public bool visitado;

	public const int stepsInfluenceExplorer = 4;
	public const int stepsInfluenceWarrior = 4;
	public const int stepsInfluenceWorker = 4;
	public const int stepsInfluenceDefensiveBuilding = 5;
	public const int stepsInfluenceBuilding = 5;
	public const int stepsInfluenceResource = 3;

    //variables para cola prioridad
    private float _prioridad;
    private int _indiceCola;

    public Node Route
    {
        get
        {
            return route;
        }

        set
        {
            route = value;
        }
    }

    public int QueuePosition
    {
        get
        {
            return queuePosition;
        }

        set
        {
            queuePosition = value;
        }
    }

    public float Cost
    {
        get
        {
            return cost;
        }

        set
        {
            cost = value;
        }
    }

    public float Estimated
    {
        get
        {
            return estimated;
        }

        set
        {
            estimated = value;
        }
    }

    public List<Pareja> ArrayVecinos
    {
        get
        {
            return arrayVecinos;
        }
    }

	public bool Water
	{
		get
		{
			return water;
		}

		set
		{
			water = value;
		}
	}

    public float prioridad
    {
        get;
        set;
    }

    public int indiceCola
    {
        get;
        set;
    }

    public Node(Vector3 pos, bool water)
    {
        queuePosition = NO_ESTA_EN_LISTA_ABIERTOS;
        Cost = float.PositiveInfinity;

        resourceType = TipoRecurso.NullResourceType;

        arrayVecinos = new List<Pareja>();
        position = pos;
        Water = water;
        route = null;

		influencePlayers = new List<List<int>> ();
		currentResources = new List<TipoRecurso> (1);
	

    }

    public void SetVecinos(Node[] vecinos)
    {
        Node nodoActual;
        float distanciaActual;

        foreach (Node value in vecinos)
        {
            if (value == null)
                continue;
            nodoActual = value;
            distanciaActual = Vector3.Distance(position, value.position);

            if (Water || nodoActual.Water)
                distanciaActual *= 5;

            if (nodoActual != null)
            {
               ArrayVecinos.Add(new Pareja(nodoActual, distanciaActual));
            }
            else
            {
                Debug.Log("Error en setVecinos");
            }
        }
    }

    public void SetVecinos(List<Pareja> vecinos)
    {
        arrayVecinos = vecinos;
    }

	//==============PARTE DE LAS INFLUENCIAS ========================

	//Usa esto CADA VEZ que quieras añadir a un jugador más a las tablas de influencias. 
	public void AddAllPlayersToInfluences()
	{
		//for (int i = 0; i < StageData.currentInstance.GetPartidaActual().numJugadores; i++) 
		for (int i = 0; i < 3; i++)
		{
			influencePlayers.Add (new List<int> (1));
		}
	}
				
	public void SetPlayerInfluence(int player, int influence)
	{
		//RECUERDA: LA POSICION 0 ESTÁ RESERVADA A LOS RECURSOS, Y LA POSICION 1 ESTÁ RESERVADA AL JUGADOR HUMANO
		if (player >= influencePlayers.Count) 
		{
			Debug.Log ("INCORRECTO: JUGADOR NO AÑADIDO ANTERIORMENTE");
			return;
		}
		else
		{
			if (player == 0) 
			{
				AddRecourseInfluence (TipoRecurso.AllTypeResource, influence);
			}
			if (influencePlayers [player] [0] < influence) 
			{
				influencePlayers [player] [0] = influence;
			}
		}
	}

	public int GetPlayerInfluence(int player)
	{
		return influencePlayers [player] [0];
	}

	//Usa esto para limpiar TODAS LAS INFLUENCIAS de un jugador, en este nodo.
	public void ClearPlayerInfluence(int player)
	{
		if (player >= influencePlayers.Count) 
		{
			Debug.Log ("INCORRECTO: JUGADOR NO AÑADIDO ANTERIORMENTE");
			return;
		}
		if (player == 0) 
		{
			currentResources.Clear ();
		}
		//Guardamos las del entorno, para ponerlas despues de borrar.
		influencePlayers [player].Clear ();
	}
		
	//Usa esto para limpiar la influencia de UN SOLO Recurso, en este nodo.
	private void ClearResourceInfluence(TipoRecurso recourseType)
	{
		if (currentResources.Contains (resourceType)) 
		{
			int indexResource = currentResources.IndexOf (recourseType);
			influencePlayers [0].RemoveAt (indexResource);
			currentResources.Remove (recourseType);
		}
	}
		
	//Usa esto para limpiar una influencia específica de un jugador, en este nodo.

	public void AddRecourseInfluence(TipoRecurso recourseType, int influence)
	{
		//Hay tipo, miramos de encontrar el máximo.
		if (currentResources.Contains (recourseType)) 
		{
			int indexResource = currentResources.IndexOf (recourseType);
			if (influencePlayers [0] [indexResource] < influence) 
			{
				influencePlayers [0] [indexResource] = influence;
			}
		} 
		// No hay tipo, añadimos ambos.
		else 
		{
			influencePlayers [0].Add (influence);
			currentResources.Add (recourseType);
		}
	}

}


