using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ResourceState
{
	Infinite, Finite, Empty
};

public class MapResource : MonoBehaviour {

	public const int MAXIMA_CANTIDAD_RECURSO = 1000;

	public TipoUnidad currentType = TipoUnidad.Resource;
	public TipoRecurso resourceType = TipoRecurso.Food;
	private Node actualNode;

	private int numJugadorOcupador = 0;
	private Jugador jugadorOcupador = null;

	private StageData stageDataReference;

	private ResourceState currentResourceState = ResourceState.Infinite;

	//SOLO A USAR CUANDO ES FINITE
	public int currentResourceQuantity; 

    //public Unidad TEST;

	// Use this for initialization
	void Start () {
       // TEST = (Unidad)this.gameObject.GetComponent<Explorador>();

		currentResourceQuantity = MAXIMA_CANTIDAD_RECURSO;

		stageDataReference = StageData.currentInstance;

		actualNode = stageDataReference.GetNodeFromPosition (this.transform.position);

    }
		
	public bool IsInfiniteResource()
	{
		return currentResourceState == ResourceState.Infinite;
	}

	public void SetResourceType( TipoRecurso nuevoTipo)
	{
		resourceType = nuevoTipo;	
	}

	public void SetOcupador (int nuevoJugador)
	{
		numJugadorOcupador = nuevoJugador;
		if (nuevoJugador != 0) 
		{
			jugadorOcupador = stageDataReference.GetPartidaActual ().Jugadores [nuevoJugador];
		}
	}

	public void DesocuparRecurso()
	{
		numJugadorOcupador = 0;
		jugadorOcupador = null;
	}

	public void SetInfluenceToAllMaps()
	{
		actualNode = stageDataReference.GetNodeFromPosition (this.transform.position);

        stageDataReference.SetInfluenceToNode(3, actualNode, numJugadorOcupador);

		/*if (jugadorOcupador != null) {
			stageDataReference.SetInfluenceToNode (Node.stepsInfluenceResource, actualNode, numJugadorOcupador,
				jugadorOcupador.influencias);
		}*/
	}

	public void ClearInfluenceToAllMaps()
	{
		actualNode = stageDataReference.GetNodeFromPosition (this.transform.position);

		stageDataReference.ClearInfluenceToNode (3, actualNode, numJugadorOcupador,
			stageDataReference.grafoTotal);

		/*if (jugadorOcupador != null) {
			stageDataReference.SetInfluenceToNode (Node.stepsInfluenceResource, actualNode, numJugadorOcupador,
				jugadorOcupador.influencias);
		}*/
	}



	// Update is called once per frame
	void Update () {

        /*
        if (Input.GetKeyDown(KeyCode.D)) {

            StageData.currentInstance.GetNodeFromPosition(transform.position).setInfluence(StageData.resourceType.Resource);


        }

        if (Input.GetKeyDown(KeyCode.A))
        {

            StageData.currentInstance.GetNodeFromPosition(transform.position).setInfluence(StageData.resourceType.Army);


        }
        */
       /* if (Input.GetKeyDown(KeyCode.S))
        {
            print(transform.position + StageData.currentInstance.grafoTotal[0, 0].position);
            StageData.currentInstance.GetPathToTarget(transform.position, StageData.currentInstance.grafoTotal[0, 0].position, this.gameObject.GetComponent<Explorador>());
			this.gameObject.GetComponent<Explorador> ().AccionMover ();
        }*/

		//if (Input.GetKeyDown (KeyCode.A)) 
		//{
		//	SetInfluenceToAllMaps ();
		//}



    }
}
