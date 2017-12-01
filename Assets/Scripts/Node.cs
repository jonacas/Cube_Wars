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

	public float influenceView = 0f;
	public float influenceResources = 0f;
	public float influenceEnemy = 0f;

	public Unidad unidad;
	public StageData.ResourceType resourceType;

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

        resourceType = StageData.ResourceType.NullResourceType;

        arrayVecinos = new List<Pareja>();
        position = pos;
        Water = water;
        route = null;
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


	public void setInfluence(StageData.UnityType resourceType)
	{
		int steps;
		switch (resourceType) 
		{
			case StageData.UnityType.Army:
			{
				steps = 5;
				setRecursiveInfluenceView (this, steps, steps);
				break;
			}
		case StageData.UnityType.Building:
			{
				steps = 5;
				setRecursiveInfluenceView (this, steps, steps);
				break;
			}
		case StageData.UnityType.Resource:
			{
				steps = 3;
				setRecursiveInfluenceResource (this, steps, steps);
				break;
			}
		}
	}

	private void setRecursiveInfluenceView(Node actualNode, float remainingSteps, float totalSteps)
	{
        Debug.Log("EntraVie");
        if (remainingSteps == 0) {
			return;
		} 
		else if (actualNode == null) 
		{
			Debug.Log ("Deberia haber un null aqui?");
			return;
		}
		else
		{
			float newInfluence = remainingSteps / totalSteps;
			if (actualNode.influenceView < newInfluence) 
			{
				actualNode.influenceView = newInfluence;
                Debug.Log("Conseguido");
			}

			for (int i = 0; i < arrayVecinos.Count; i++) 
			{
                
				setRecursiveInfluenceView (actualNode.arrayVecinos[i].nodo, remainingSteps - 1, totalSteps);
                Debug.Log("Entra");
			}
		}
	}

	private void setRecursiveInfluenceResource(Node actualNode, float remainingSteps, float totalSteps)
	{
        Debug.Log("EntraRes");
        if (remainingSteps == 0) {
			return;
		} 
		else if (actualNode == null) 
		{
			Debug.Log ("Deberia haber un null aqui?");
			return;
		}
		else
		{
			float newInfluence = remainingSteps / totalSteps;
			if (actualNode.influenceResources < newInfluence) 
			{
				actualNode.influenceResources = newInfluence;
                Debug.Log("Conseguido");
            }

			for (int i = 0; i < arrayVecinos.Count; i++) 
			{
				setRecursiveInfluenceView (actualNode.arrayVecinos[i].nodo, remainingSteps - 1, totalSteps);		
			}
		}
	}

	private void setRecursiveInfluenceEnemy(Node actualNode, float remainingSteps, float totalSteps)
	{
        Debug.Log("EntraEne");
        if (remainingSteps == 0) {
			return;
		} 
		else if (actualNode == null) 
		{
			Debug.Log ("Deberia haber un null aqui?");
			return;
		}
		else
		{
			float newInfluence = remainingSteps / totalSteps;
			if (actualNode.influenceEnemy < newInfluence) 
			{
				actualNode.influenceEnemy = newInfluence;
			}

			for (int i = 0; i < arrayVecinos.Count; i++) 
			{
				setRecursiveInfluenceView (actualNode.arrayVecinos[i].nodo, remainingSteps - 1, totalSteps);		
			}
		}
	}




}
