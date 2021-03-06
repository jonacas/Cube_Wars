﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum TipoUnidad
{
    Warrior,
    Worker,
    Explorer,
    Resource,
    Building,
    DefensiveBuilding,
    Capital
};

public enum TipoRecurso
{
    Food,
    Wood,
    Steel,
    Rock,
    NullResourceType,
	AllTypeResource
};


public class StageData : MonoBehaviour
{

    public GameObject player, WarriorPrefab, WorkerPrefab, ExplorerPrefab, TowerPrefab, ResourceBuildPrefab , MistPrefab;
    public Material materialNiebla;
    public GameObject WoodBuildPrefab, StoneBuildPrefab, MetalBuildPrefab;
    public GameObject explorerIAPrefab, aldeanoIAprefab, guerreroIAPrefab;
    public static StageData currentInstance;
    public List<EnemyMovement> enemiesInStage;
    public CreacionGrafo CG;
    public TipoUnidad unidadACrear;
	public Material[] colores = new Material[4];

	public List<MapResource> testResourceList;

	public List<GameObject> mapResourceReference;

    #region COSTES_PA
    public const int COSTE_PA_CONSTRUIR_RECURSOS = 20;
	public const int COSTE_PA_CONSTRUIR_TORRE = 20;
	public const int COSTE_PA_CREAR_ALDEANO = 20;
	public const int COSTE_PA_CREAR_GUERRERO = 20;
	public const int COSTE_PA_MOVER_UNIDAD = 20;
	public const int COSTE_PA_ATACAR = 20;
    #endregion

    #region SALUDES
    public const int SALUD_MAX_GUERRERO = 100;
    public const int SALUD_MAX_EXPLORADOR = 100;
    public const int SALUD_MAX_ALDEANO = 100;
    public const int SALUD_MAX_TORRE_DEFENSIVA = 100;
    public const int SALUD_MAX_CAPITAL = 500;
    public const int SALUD_MAX_RECOLECTOR = 50;
    #endregion

    #region DEFENSAS
    public const int DEFENSA_MAX_GUERRERO = 50;
    public const int DEFENSA_MAX_EXPLORADOR = 20;
    public const int DEFENSA_MAX_ALDEANO = 20;
    public const int DEFENSA_MAX_TORRE_DEFENSIVA = 20;
    public const int DEFENSA_MAX_CAPITAL = 20;
    public const int DEFENSA_MAX_RECOLECTOR = 20;
    #endregion

    #region ATAQUES
    public const int ATAQUE_GUERRERO = 50;
    public const int ATAQUE_TORRE_DEFENSIVA = 80;
    #endregion

    #region RECURSOS_INICIALES
    public const int MADERA_INICIAL = 50;
    public const int METAL_INICIAL = 80;
    public const int PIEDRA_INICIAL = 50;
    public const int COMIDA_INICIAL = 80;
    #endregion

    public const int ID_JUGADOR_HUMANO = 0;

    public const int CANTIDAD_RECOLECTADA = 50;

	public const int CANTIDAD_RECURSOS_MAPA = 150;

    private int obstacleLayerMask = 1 << 8;
    //private bool cmunicationsEnabeled;
    public bool ComunicationsEnabeled
    {
        get;
        set;
    }

    private AEstrella aStar;

    public GameObject[] tobeInteractedList;
    private bool[] pressedButtons = { false, false, false };

    public Node LastClickedNode;
	public Node[,] grafoTotal;

	//0 == recursos, 1 == jugador humano, 2 == IA 1
	public int numberOfPlayers = 3;

    private Partida partidaActual;

	//public GameObject UnitGamePanelPrefab;

	public GameObject testCasillaMarcada;

	public GameObject testModeladoResource;
	public GameObject FoodResourceModel;
	public GameObject WoodResourceModel;
	public GameObject StoneResourceModel;
	public GameObject MetalResourceModel;

    GameObject piscinaNiebla;

    void Awake()
    {
        currentInstance = this;
        ComunicationsEnabeled = true;
        aStar = this.GetComponent<AEstrella>();
		grafoTotal = CG.nodeMap;
		mapResourceReference = new List<GameObject> ();
    }
    public GameObject GetPlayer()
    {
        return player;
    }
    public void SetNieblaDeGuerra(int idJugadorActual)
    {

        Destroy(piscinaNiebla);
        piscinaNiebla = new GameObject("PiscinaNiebla");
        GameObject nieblaActual;
        foreach (Node n in grafoTotal)
        {
            if (n != null)
                switch (n.GetPlayerInfluence(idJugadorActual))
                {
                    case -1:
                        nieblaActual = Instantiate(MistPrefab, n.position, MistPrefab.transform.rotation, piscinaNiebla.transform);
                        break;
                    case 0:
                        if (n.resourceType != TipoRecurso.NullResourceType || (n.unidad != null && (n.unidad.IdUnidad == TipoUnidad.Resource || n.unidad.IdUnidad == TipoUnidad.DefensiveBuilding)))
                        {
                            //Debug.Log("PATATA VOLADORA"); //NO ESTA ENTRANDO LOL!!!!!!!
                            nieblaActual = Instantiate(MistPrefab, n.position, MistPrefab.transform.rotation, piscinaNiebla.transform);
                            nieblaActual.GetComponent<Renderer>().material = materialNiebla;
                        }
                        else
                        {
                            nieblaActual = Instantiate(MistPrefab, n.position, MistPrefab.transform.rotation, piscinaNiebla.transform);
                        }
                        break;
                    default:
                        break;
                }
        }
    }


    public void ClearNieblaDeGuerra()
    {
        Destroy(piscinaNiebla);
    }

    public Node GetNodeFromPosition(Vector3 requester)
	{
		///en base a las coordenadas se obtiene la fila y columna de los nodos
		int initX = (int)Mathf.Round(requester.x / CG.incrementoX);
		int initZ = (int)Mathf.Round(requester.z / CG.incrementoZ);

		if (initX < 0 || initX >= CG.filas || initZ < 0 || initZ >= CG.columnas) 
		{
			return null;
		}
        if (grafoTotal[initX, initZ] == null) { return null; }
        LastClickedNode = grafoTotal[initZ, initX];
		return grafoTotal [initZ, initX];


	}    

    public List<Vector3> GetPathToTarget(Vector3 requester, Vector3 target, Unidad solicitante)
    {
        List<Pareja> listaVecinosAux = new List<Pareja>();

        Node inicio, final;

        /*PARTE QUE DETERMINA EL NODO*/
        ///en base a las coordenadas se obtiene la fila y columna de los nodos
        int initX = (int)Mathf.Round(requester.x / CG.incrementoX);
        int initZ = (int)Mathf.Round(requester.z / CG.incrementoZ);
        int finalX = (int)Mathf.Round(target.x / CG.incrementoX);
        int finalZ = (int)Mathf.Round(target.z / CG.incrementoZ);

        if (initX < 0 || initX >= CG.filas || initZ < 0 || initZ >= CG.columnas || finalX < 0 || finalX >= CG.filas || finalZ < 0 || finalZ >= CG.columnas)
            return null;

        /*FIN DE PARTE QUE DETERMINA EL NODO*/

        Node closestNode = null;

        if (grafoTotal[initZ, initX] != null)
            inicio = grafoTotal[initZ, initX];
        else
            inicio = null;

        if (inicio == null)
        {
            closestNode = null;
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if (grafoTotal[initZ + i, initX + j] != null)
                        inicio = grafoTotal[initZ + i, initX + j];
                    else
                        inicio = null;

                    if (inicio != null && !Physics.Raycast(grafoTotal[initZ + i, initX + j].position, target,
                        Vector3.Distance(grafoTotal[initZ + i, initX + j].position, target), obstacleLayerMask))
                    {
                        if (closestNode == null)
                        {
                            closestNode = inicio;
                        }
                        else if (Vector3.Distance(target, inicio.position) < Vector3.Distance(target, closestNode.position))
                        {
                            closestNode = inicio;
                        }

                    }
                }
            }
            inicio = closestNode;
        }

        if (grafoTotal[finalZ, finalX] != null)
            final = grafoTotal[finalZ, finalX];
        else
            final = null;

        if (final == null)
        {
            closestNode = null;
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if (grafoTotal[finalZ + i, finalX + j] != null)
                        final = grafoTotal[finalZ + i, finalX + j];
                    else
                        final = null;

                    if (final != null && !Physics.Raycast(grafoTotal[finalZ + i, finalX + j].position, target,
                            Vector3.Distance(grafoTotal[finalZ + i, finalX + j].position, target), obstacleLayerMask))
                    {
                        if (closestNode == null)
                        {
                            closestNode = final;
                        }
                        else if (Vector3.Distance(target, final.position) < Vector3.Distance(target, closestNode.position))
                        {
                            closestNode = final;
                        }

                    }
                }
            }
            final = closestNode;
        }
        aStar.FindPath(final, inicio, CG.filas * CG.columnas, false, true, solicitante, grafoTotal);

        return new List<Vector3>();
    }

	/*public void SetUnidadToNode( Unidad objeto)
	{
		GetNodeFromPosition (objeto.transform.position).unidad = objeto;
	}*/

	/*private void SetResourceTypeToNode (TipoRecurso type, Vector3 resourcePosition)
	{
		GetNodeFromPosition (resourcePosition).resourceType = type;
	}*/


	//USA ESTA FUNCION PARA SETTEAR LA INFLUENCIA DESDE EL CENTRO DEL NODO, PARA UN JUGADOR.
	public void SetInfluenceToNode(int numberOfSteps, Node center, int player)
	{
		int posX;
		int posY;

		int numberOfStepsCuadratic = (numberOfSteps * 2 + 1);
		int steps1 = (int) numberOfStepsCuadratic / 2;
		int stepsDif = numberOfStepsCuadratic - steps1;

        Node[,] grafo = StageData.currentInstance.grafoTotal;

		//center.SetPlayerInfluence (player, numberOfSteps);
		//Debug.Log ("peso en" + " x: " + 0 + " , " + "y: " + 0 + "==>" +  (numberOfSteps) );

		for (int i = -steps1; i < stepsDif; i++) 
		{
			//Debug.Log ("num ejecuciones bucle fila");

			for (int j = -steps1; j < stepsDif; j++) 
			{
				posX = center.fil + i;
				posY = center.col + j;

				//Debug.Log ("num ejecuciones bucle COLUMNA");

				if (posX < 0 || posX >= CG.filas) {	continue;	}
				else if (posY < 0 || posY >= CG.columnas) {	continue;}
				else
				{	//posicion legal, ahora viene la paja.
					int difX = (int)Mathf.Abs(center.fil - posX); 
					int difY = (int)Mathf.Abs (center.col - posY);
					if (numberOfSteps - difY == 0 || numberOfSteps - difX == 0) {continue;	}
					if (difX <= difY) 
					{
                        if (grafo[posX, posY] == null)
                            continue;
						grafo [posX, posY].SetPlayerInfluence (player, numberOfSteps - difY);
						if (grafo [posX, posY].resourceType != TipoRecurso.NullResourceType) 
						{
							partidaActual.Jugadores [player].RecursoEncontrado (grafo [posX, posY].position);
						}
						
						//Debug.Log ("peso en" + " x: " + difX + " , " + "y: " + difY + "==>" +  grafo[posX, posY].GetPlayerInfluence(player));
					}
					else 
					{
                        if (grafo[posX, posY] == null)
                            continue;
                        grafo [posX, posY].SetPlayerInfluence (player, numberOfSteps - difX);
                       
                        if (grafo [posX, posY].resourceType != TipoRecurso.NullResourceType) 
						{
							partidaActual.Jugadores [player].RecursoEncontrado (grafo [posX, posY].position);
						}
						//Debug.Log ("peso en" + " x: " + difX + " , " + "y: " + difY + "==>" +  grafo[posX, posY].GetPlayerInfluence(player));
					}
				}
			}
		}
	}

	//USA ESTA FUNCION PARA LIMPIAR LA INFLUENCIA DEL JUGADOR DESDE EL CENTRO DEL NODO.
	public void ClearInfluenceToNode(int numberOfSteps, Node center, int player, Node[,] grafo)
	{
		int posX;
		int posY;

		int numberOfStepsCuadratic = (numberOfSteps * 2 + 1);
		int steps1 = (int) numberOfStepsCuadratic / 2;
		int stepsDif = numberOfStepsCuadratic - steps1;

		//center.ClearPlayerInfluence (player);
		//Debug.Log ("peso en" + " x: " + 0 + " , " + "y: " + 0 + "==>" +  (numberOfSteps) );

		for (int i = -steps1; i < stepsDif; i++) 
		{
			//Debug.Log ("num ejecuciones bucle fila");

			for (int j = -steps1; j < stepsDif; j++) 
			{
				posX = center.fil + i;
				posY = center.col + j;

				//Debug.Log ("num ejecuciones bucle COLUMNA");

				if (posX < 0 || posX >= CG.filas) {	continue;	}
				else if (posY < 0 || posY >= CG.columnas) {	continue;}
				else
				{   //posicion legal, ahora viene la paja.
                    int difX = (int)Mathf.Abs(center.fil - posX);
                    int difY = (int)Mathf.Abs(center.col - posY);
                    if (numberOfSteps - difY == 0 || numberOfSteps - difX == 0) { continue; }

                    if (grafo[posX, posY] == null)
                        continue;
                    if (difX <= difY)
                    {
                        grafo[posX, posY].SetPlayerInfluence(player, difY - numberOfSteps);
                        //Debug.Log ("peso en" + " x: " + difX + " , " + "y: " + difY + "==>" +  grafo[posX, posY].GetPlayerInfluence(player));
                    }
                    else
                    {
                        grafo[posX, posY].SetPlayerInfluence(player, difX - numberOfSteps);
                    }
                }
			}
		}
	}

	public void ClearAllResourceInfluence()
	{
		for (int i = 0; i < testResourceList.Count; i++) 
		{
			testResourceList [i].ClearInfluenceToAllMaps();
		}
	}
	public void SetAllResourceInfluence()
	{
		for (int i = 0; i < testResourceList.Count; i++) 
		{
			testResourceList [i].SetInfluenceToAllMaps ();
		}

		foreach (Node n in grafoTotal) 
		{
			if (n.GetPlayerInfluence(0) != -1) 
			{
				GameObject nuevo = Instantiate (testCasillaMarcada,  grafoTotal [n.fil, n.col].position, Quaternion.identity);
				if (nuevo.GetComponent<TextMesh> () != null) 
				{
					nuevo.GetComponent<TextMesh> ().text = (n.GetPlayerInfluence(0)).ToString();
				}
			}
		}
	}

	void Update()
	{
		if (Input.GetKeyDown (KeyCode.A)) 
		{
			//SetAllResourceInfluence ();
		}
		if (Input.GetKeyDown (KeyCode.C)) 
		{
			//SetInitialResourcesOnMap ();
			//ClearAllResourceInfluence ();
		}
	}


    public void LimpiarGrafo(Node[,] nodeMap)
    {
        foreach (Node n in nodeMap)
        {
            if (n != null)
                n.Cost = float.PositiveInfinity;
        }

    }

    public Partida GetPartidaActual()
    {
        return partidaActual;
    }

    public void SetPartidaActual(Partida par)
    {
        partidaActual = par;
    }


	public void SetInitialResourcesOnMap()
	{
		for (int i = 0; i < CANTIDAD_RECURSOS_MAPA; i++) 
		{
			int randFil = Random.Range (0, CG.filas - 1);
			int randCol = Random.Range (0, CG.columnas - 1);
			if (grafoTotal[randFil, randCol] == null || 
				grafoTotal [randFil, randCol].unidad != null || 
				grafoTotal[randFil, randCol].resourceType != TipoRecurso.NullResourceType) {	i = i - 1;	continue;	}
			else
			{
				//AQUI ESCOGEMOS UN MODELADO ALEATORIO A PONER.
				int randomResource = Random.Range (0, 4);
				Vector3 resourcePosition = new Vector3 (grafoTotal [randFil, randCol].position.x, grafoTotal [randFil, randCol].position.y - 1, grafoTotal [randFil, randCol].position.z);
				switch (randomResource) 
				{
					case 0:
					{
						GameObject modeladoTest = Instantiate (FoodResourceModel, resourcePosition, FoodResourceModel.transform.rotation);
						mapResourceReference.Add (modeladoTest);
                        grafoTotal[randFil, randCol].SetResourceToNode(TipoRecurso.Food);
                            break;
					}
					case 1:
					{
						GameObject modeladoTest = Instantiate (WoodResourceModel, resourcePosition, WoodResourceModel.transform.rotation);
						mapResourceReference.Add (modeladoTest);
                        grafoTotal[randFil, randCol].SetResourceToNode(TipoRecurso.Wood);
                            break;
					}
					case 2:
					{
						GameObject modeladoTest = Instantiate (MetalResourceModel, resourcePosition, MetalResourceModel.transform.rotation);
						mapResourceReference.Add (modeladoTest);
                        grafoTotal[randFil, randCol].SetResourceToNode(TipoRecurso.Steel);
                            break;
					}
					case 3:
					{
						GameObject modeladoTest = Instantiate (StoneResourceModel, resourcePosition, StoneResourceModel.transform.rotation);
						mapResourceReference.Add (modeladoTest);
                        grafoTotal[randFil, randCol].SetResourceToNode(TipoRecurso.Rock);
                            break;
					}
				}
				//grafoTotal [randFil, randCol].SetPlayerInfluence (0, Node.stepsInfluenceResource);
			}
		}
	}


	public void RemoveResourceModel(Vector3 position)
	{
		//Debug.Log (mapResourceReference.Count);
		for (int i = 0; i < mapResourceReference.Count; i++) 
		{
			if (mapResourceReference [i].transform.position.x == position.x && mapResourceReference [i].transform.position.z == position.z) 
			{
				GameObject toBeDestroyed = mapResourceReference [i];
				mapResourceReference.RemoveAt (i);
				Destroy (toBeDestroyed);
				return;
			}
		}
		//Debug.Log ("has tratado de eliminar un recurso que no existe o ya se ha eliminado antes: revisa el codigo.");

	}

    public void SetBuildingPlayerInfluence()
    {
        Jugador ActualPlayer = GetPartidaActual().JugadorActual;
        List<Node> alcance;
        for (int i = 0; i < ActualPlayer.edificios.Count; i++)
        {
            alcance = Control.GetNodosAlAlcance(ActualPlayer.edificios[i].Nodo, Node.stepsInfluenceBuilding);
            for (int j = 0; j < alcance.Count; j++)
            {
                if (alcance[j].GetPlayerInfluence(ActualPlayer.idJugador) <= 0)
                {
                    SetInfluenceToNode(Node.stepsInfluenceBuilding, ActualPlayer.edificios[i].Nodo, ActualPlayer.idJugador);
                    break;
                }
            }
        }
        alcance = Control.GetNodosAlAlcance(ActualPlayer.Capital.Nodo, Node.stepsInfluenceBuilding);
        for (int j = 0; j < alcance.Count; j++)
        {
            if (alcance[j].GetPlayerInfluence(ActualPlayer.idJugador) <= 0)
            {
                SetInfluenceToNode(Node.stepsInfluenceBuilding, ActualPlayer.Capital.Nodo, ActualPlayer.idJugador);
                break;
            }
        }
    }








}
