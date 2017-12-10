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

	public int stepsInfluenceExplorer = 4;
	public int stepsInfluenceWarrior = 4;
	public int stepsInfluenceWorker = 4;
	public int stepsInfluenceDefensiveBuilding = 5;
	public int stepsInfluenceBuilding = 5;
	public int stepsInfluenceResource = 3;

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
		currentResources = new List<TipoRecurso> ();

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
	public void AddPlayerToInfluences()
	{
		influencePlayers.Add (new List<int> ());
		Debug.Log ("Player " + influencePlayers.Count + " added to estimated influences");
	}

	//Usa este para poner la influencia de las unidades a sus vecinos, por cada jugador.
	public void SetInfluence(TipoUnidad resourceType, int player)
	{
		switch (resourceType) 
		{
		case TipoUnidad.Warrior:
			{
				setRecursiveInfluenceView (this,player, 0, stepsInfluenceWarrior);
				break;
			}
		case TipoUnidad.Worker:
			{
				setRecursiveInfluenceView (this, player, 0, stepsInfluenceWorker);
				break;
			}
		case TipoUnidad.Explorer:
			{
				setRecursiveInfluenceView (this,player, 0, stepsInfluenceExplorer);
				break;
			}
		case TipoUnidad.Building:
			{
				setRecursiveInfluenceView (this,player, 0, stepsInfluenceBuilding);
				break;
			}
		case TipoUnidad.DefensiveBuilding:
			{
				setRecursiveInfluenceView (this,player, 0, stepsInfluenceDefensiveBuilding);
				break;
			}
		case TipoUnidad.Resource:
			{
				Debug.Log ("Funcion para settear resources equivocada, usa setResourceInfluence()");
				break;
			}
		}
	}

	//Usa este para poner la influencia de los recursos a sus vecinos, por cada uno de ellos.
	public void SetResourceInfluence(TipoRecurso resource)
	{
		setRecursiveInfluenceResource (this, resource, 0, stepsInfluenceResource);
	}

	private void setRecursiveInfluenceResource(Node actualNode, TipoRecurso resource, int remainingSteps, int totalSteps)
	{
		Debug.Log("EntraVie");
		if (remainingSteps == totalSteps) {
			return;
		} 
		else if (actualNode == null) 
		{
			Debug.Log ("Deberia haber un null aqui?");
			return;
		}
		else
		{
			int newInfluence = totalSteps - remainingSteps;
			AddRecourseInfluence (resource, newInfluence);
			for (int i = 0; i < arrayVecinos.Count; i++) 
			{
				setRecursiveInfluenceResource(actualNode.arrayVecinos[i].nodo, resource, remainingSteps + 1, totalSteps);
				Debug.Log("Entra");
			}
		}
	}

	private void setRecursiveInfluenceView(Node actualNode, int player, int remainingSteps, int totalSteps)
	{
        Debug.Log("EntraVie");
		if (remainingSteps == totalSteps) {
			return;
		} 
		else if (actualNode == null) 
		{
			Debug.Log ("Deberia haber un null aqui?");
			return;
		}
		else
		{
			int newInfluence = totalSteps - remainingSteps;
			SetPlayerInfluence (player, newInfluence);
			for (int i = 0; i < arrayVecinos.Count; i++) 
			{
				setRecursiveInfluenceView (actualNode.arrayVecinos[i].nodo,player, remainingSteps + 1, totalSteps);
                Debug.Log("Entra");
			}
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
			influencePlayers [player].Add (influence);
		}
	}

	//Usa esto para limpiar la influencia de las unidades a sus vecinos, por cada jugador.
	public void ClearInfluence(TipoUnidad resourceType, int player)
	{
		switch (resourceType) 
		{
		case TipoUnidad.Warrior:
			{
				clearRecursiveInfluenceView (this, player, stepsInfluenceWarrior, stepsInfluenceWarrior);
				break;
			}
		case TipoUnidad.Worker:
			{
				clearRecursiveInfluenceView (this,player, stepsInfluenceWorker, stepsInfluenceWorker);
				break;
			}
		case TipoUnidad.Explorer:
			{
				clearRecursiveInfluenceView (this,player, stepsInfluenceExplorer, stepsInfluenceExplorer);
				break;
			}
		case TipoUnidad.Building:
			{
				clearRecursiveInfluenceView (this,player, stepsInfluenceBuilding, stepsInfluenceBuilding);
				break;
			}
		case TipoUnidad.DefensiveBuilding:
			{
				clearRecursiveInfluenceView (this,player, stepsInfluenceDefensiveBuilding, stepsInfluenceDefensiveBuilding);
				break;
			}
		case TipoUnidad.Resource:
			{
				Debug.Log ("Funcion para settear resources equivocada, usa clearResourceInfluence()");
				break;
			}
		}
	}

	//Usa esto para limpiar la influencia de TODOS LOS RECURSOS a sus vecinos.
	public void ClearAllResourceInfluence()
	{
		clearRecursiveInfluenceResource (this, 0, stepsInfluenceResource);
	}

	private void clearRecursiveInfluenceResource(Node actualNode, int remainingSteps, int totalSteps)
	{
		Debug.Log("EntraVie");
		if (remainingSteps == totalSteps) {
			return;
		} 
		else if (actualNode == null) 
		{
			Debug.Log ("Deberia haber un null aqui?");
			return;
		}
		else
		{
			ClearAllPlayerInfluence (0);
			for (int i = 0; i < arrayVecinos.Count; i++) 
			{
				if (influencePlayers [0].Count != 0) 
				{
					clearRecursiveInfluenceResource(actualNode.arrayVecinos[i].nodo, remainingSteps + 1, totalSteps);
					Debug.Log("Entra");
				}
			}
		}
	}

	private void clearRecursiveInfluenceView(Node actualNode, int player, int remainingSteps, int totalSteps)
	{
		//Debug.Log("EntraVie");
		if (remainingSteps == 0) {
			return;
		} 
		else if (actualNode == null) 
		{
			//Debug.Log ("Deberia haber un null aqui?");
			return;
		}
		else
		{
			ClearAllPlayerInfluence (player);
				//Debug.Log("Conseguido");

			for (int i = 0; i < arrayVecinos.Count; i++) 
			{
				if (actualNode.arrayVecinos [i].nodo.influencePlayers [player].Count != 0) 
				{
					clearRecursiveInfluenceView (actualNode.arrayVecinos[i].nodo,player, remainingSteps - 1, totalSteps);
					//Debug.Log("Entra");
				}
			}
		}
	}

	//Usa esto para limpiar TODAS LAS INFLUENCIAS de un jugador, en este nodo.
	public void ClearAllPlayerInfluence(int player)
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

	//Usa esto para limpiar un tipo concreto de recurso en sus vecinos. (p ej, cuando el recurso se acaba).
	//CUIDADO: al eliminarse, todos los recursos del mismo tipo de su alrededor deben actualizar sus influencias.
	public void ClearSpecificResource(TipoRecurso recourseType)
	{
		clearSpecificRecursiveResource (this,resourceType, 0, stepsInfluenceResource);
	}

	private void clearSpecificRecursiveResource(Node actualNode, TipoRecurso recourse, int remainingSteps, int totalSteps)
	{
		Debug.Log("EntraVie");
		if (remainingSteps == totalSteps) {
			return;
		} 
		else if (actualNode == null) 
		{
			Debug.Log ("Deberia haber un null aqui?");
			return;
		}
		else
		{
			if (currentResources.Contains (recourse))  // Tiene recurso que buscamos.
			{
				//Miramos si es necesario quitarlo: de haber más de uno disponible, this is bad!
				int currentInfluence = totalSteps - remainingSteps;
				int resourceIndex = currentResources.IndexOf (recourse);
				if (influencePlayers [0] [resourceIndex] == currentInfluence) 
				{
					ClearResourceInfluence (recourse);
				}
			}
			for (int i = 0; i < arrayVecinos.Count; i++) 
			{
					clearSpecificRecursiveResource(actualNode.arrayVecinos[i].nodo, recourse, remainingSteps + 1, totalSteps);
					Debug.Log("Entra");
			}
		}
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

	//usa esto para obtener la influencia máxima de un jugador en este nodo.
	//Sirve también para saber la influencia máxima de los recursos, 
	//Pero para ello necesitarás encontrar según su resultado el tipo de recurso si quieres que sea util.
	public int GetMaxInfluenceFromPlayer(int player)
	{
		if (player >= influencePlayers.Count) 
		{
			Debug.Log ("INCORRECTO: JUGADOR NO AÑADIDO ANTERIORMENTE");
			return -1;
		}
		else
		{
			int resultado = 0;
			for (int i = 0; i < influencePlayers [player].Count; i++) 
			{
				if (resultado < influencePlayers [player] [i]) 
				{
					resultado = influencePlayers [player] [i];
				}
			}
			return resultado;
		}
	}

	//Usa esto para limpiar una influencia específica de un jugador, en este nodo.
	public void ClearSpecificInfluenceFromPlayer(int player, int influence)
	{
		if (player >= influencePlayers.Count) 
		{
			Debug.Log ("INCORRECTO: JUGADOR NO AÑADIDO ANTERIORMENTE");
			return;
		}
		else
		{
			for (int i = 0; i < influencePlayers [player].Count; i++) 
			{
				if (influencePlayers [player].Remove (influence)) 
				{
					Debug.Log ("Borrado de influencia de " +
					"unidad con influencia " + influence + " de jugador " + player);
				}
				else
				{
					Debug.Log ("BORRADO NO EXITOSO: Valor no correcto?");	
				}
			}
		}
	}

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
		
	public List<int> GetAllInfluencesFromPlayer(int player)
	{
		return influencePlayers [player];
	}


}


