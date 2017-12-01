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
    DefensiveBuilding
};

public enum TipoRecurso
{
    Food,
    Wood,
    Steel,
    Rock,
    NullResourceType
};


public class StageData : MonoBehaviour
{

    public GameObject player;
    public static StageData currentInstance;
    public List<EnemyMovement> enemiesInStage;
    public CreacionGrafo CG;

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
	public Node[,] grafoJugador1;
	public Node[,] grafoJugador2;


    void Awake()
    {
        currentInstance = this;
        ComunicationsEnabeled = true;
        aStar = this.GetComponent<AEstrella>();
		grafoTotal = CG.nodeMap;
		grafoJugador1 = grafoTotal;
		grafoJugador2 = grafoTotal;
    }
    public GameObject GetPlayer()
    {
        return player;
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
    /*
    public void SendAlert(Vector3 detectedPos, int stage, int area)
    {
        if (ComunicationsEnabeled)
        {
            //print("Sending alert...");
            for (int i = 0; i < enemiesInStage.Count; i++)
            {
                if (enemiesInStage[i].enemyIDStage == stage && enemiesInStage[i].enemyIDStagePart == area)
                {
                    enemiesInStage[i].SendAlertToPosition(detectedPos);
                }
            }

            sendAlertToOtherZones(stage, area);
        }
    }
    /*
    private void sendAlertToOtherZones(int area, int stage)
    {
        print("Alerta a otras zonas");
        switch (area)
        {
            case 1:
                {
                    GameObject.Find("DR QB 2 1").GetComponent<EnemyMovement>().AlertFromAnotherZone(GameObject.Find("2_1").transform.position);
                    GameObject.Find("DR QB 2 2").GetComponent<EnemyMovement>().AlertFromAnotherZone(GameObject.Find("2_1").transform.position);
                    break;
                }

            case 2:
                {
                    GameObject.Find("DR QB 1 1").GetComponent<EnemyMovement>().AlertFromAnotherZone(GameObject.Find("1_2").transform.position);
                    GameObject.Find("DR QB 1 2").GetComponent<EnemyMovement>().AlertFromAnotherZone(GameObject.Find("1_2").transform.position);
                    GameObject.Find("DR QB 3 1 1").GetComponent<EnemyMovement>().AlertFromAnotherZone(GameObject.Find("31_2").transform.position);
                    GameObject.Find("DR QB 3 1 2").GetComponent<EnemyMovement>().AlertFromAnotherZone(GameObject.Find("31_2").transform.position);
                    break;
                }

            case 3:
                {
                    switch (stage)
                    {
                        case 1:
                            GameObject.Find("DR QB 2 1").GetComponent<EnemyMovement>().AlertFromAnotherZone(GameObject.Find("31_2").transform.position);
                            GameObject.Find("DR QB 2 2").GetComponent<EnemyMovement>().AlertFromAnotherZone(GameObject.Find("31_2").transform.position);
                            GameObject.Find("DR QB 3 2 1").GetComponent<EnemyMovement>().AlertFromAnotherZone(GameObject.Find("31_32").transform.position);
                            GameObject.Find("DR QB 3 2 2").GetComponent<EnemyMovement>().AlertFromAnotherZone(GameObject.Find("31_32").transform.position);
                            GameObject.Find("DR QB 3 4 1").GetComponent<EnemyMovement>().AlertFromAnotherZone(GameObject.Find("31_34").transform.position);
                            break;
                        case 2:
                            GameObject.Find("DR QB 3 1 1").GetComponent<EnemyMovement>().AlertFromAnotherZone(GameObject.Find("32_31").transform.position);
                            GameObject.Find("DR QB 3 1 2").GetComponent<EnemyMovement>().AlertFromAnotherZone(GameObject.Find("32_31").transform.position);
                            GameObject.Find("DR QB 3 3 1").GetComponent<EnemyMovement>().AlertFromAnotherZone(GameObject.Find("32_33").transform.position);
                            GameObject.Find("DR QB 3 3 2").GetComponent<EnemyMovement>().AlertFromAnotherZone(GameObject.Find("32_33").transform.position);
                            break;
                        case 3:
                            GameObject.Find("DR QB 3 2 1").GetComponent<EnemyMovement>().AlertFromAnotherZone(GameObject.Find("33_32").transform.position);
                            GameObject.Find("DR QB 3 2 2").GetComponent<EnemyMovement>().AlertFromAnotherZone(GameObject.Find("33_32").transform.position);
                            GameObject.Find("DR QB 3 4 1").GetComponent<EnemyMovement>().AlertFromAnotherZone(GameObject.Find("33_34").transform.position);
                            break;
                        case 4:
                            break;
                    }
                    break;
                }
        }

    }

    public void CancelAlertToOtherZones(int area, int stage)
    {
        switch (area)
        {
            case 1:
                {
                    GameObject.Find("DR QB 2 1").GetComponent<EnemyMovement>().CancelAlertFromAnotherZone();
                    GameObject.Find("DR QB 2 2").GetComponent<EnemyMovement>().CancelAlertFromAnotherZone();
                    break;
                }

            case 2:
                {
                    GameObject.Find("DR QB 1 1").GetComponent<EnemyMovement>().CancelAlertFromAnotherZone();
                    GameObject.Find("DR QB 1 2").GetComponent<EnemyMovement>().CancelAlertFromAnotherZone();
                    GameObject.Find("DR QB 3 1 1").GetComponent<EnemyMovement>().CancelAlertFromAnotherZone();
                    GameObject.Find("DR QB 3 1 2").GetComponent<EnemyMovement>().CancelAlertFromAnotherZone();
                    break;
                }

            case 3:
                {
                    switch (stage)
                    {
                        case 1:
                            GameObject.Find("DR QB 2 1").GetComponent<EnemyMovement>().CancelAlertFromAnotherZone();
                            GameObject.Find("DR QB 2 2").GetComponent<EnemyMovement>().CancelAlertFromAnotherZone();
                            GameObject.Find("DR QB 3 2 1").GetComponent<EnemyMovement>().CancelAlertFromAnotherZone();
                            GameObject.Find("DR QB 3 2 2").GetComponent<EnemyMovement>().CancelAlertFromAnotherZone();
                            GameObject.Find("DR QB 3 4 1").GetComponent<EnemyMovement>().CancelAlertFromAnotherZone();
                            break;
                        case 2:
                            GameObject.Find("DR QB 3 1 1").GetComponent<EnemyMovement>().CancelAlertFromAnotherZone();
                            GameObject.Find("DR QB 3 1 2").GetComponent<EnemyMovement>().CancelAlertFromAnotherZone();
                            GameObject.Find("DR QB 3 3 1").GetComponent<EnemyMovement>().CancelAlertFromAnotherZone();
                            GameObject.Find("DR QB 3 3 2").GetComponent<EnemyMovement>().CancelAlertFromAnotherZone();
                            break;
                        case 3:
                            GameObject.Find("DR QB 3 2 1").GetComponent<EnemyMovement>().CancelAlertFromAnotherZone();
                            GameObject.Find("DR QB 3 2 2").GetComponent<EnemyMovement>().CancelAlertFromAnotherZone();
                            GameObject.Find("DR QB 3 4 1").GetComponent<EnemyMovement>().CancelAlertFromAnotherZone();
                            break;
                        case 4:
                            break;
                    }
                    break;
                }
        }

    }

    public void SendNoise(Vector3 noisePos, float hearingRange)
    {
        for (int i = 0; i < enemiesInStage.Count; i++)
        {
            if (Vector3.Distance(enemiesInStage[i].gameObject.transform.position, noisePos) < hearingRange)
                enemiesInStage[i].SendAlertToPosition(noisePos);
        }
        //lo mismo que send alert, pero esta vez no depende de comunicaciones, si no de rango
    }

    public void PressedButton(string buttonName)
    {
        switch (buttonName)
        {
            case "Boton1":
                {
                    tobeInteractedList[0].SetActive(false);
                    pressedButtons[0] = true;
                    break;
                }
            case "Boton2":
                {
                    tobeInteractedList[1].SetActive(false);
                    pressedButtons[1] = true;
                    break;
                }
            case "Boton3":
                {
                    tobeInteractedList[2].SetActive(false);
                    pressedButtons[2] = true;
                    break;
                }
            case "BotonComunicaciones":
                {
                    StageData.currentInstance.ComunicationsEnabeled = false;
                    break;
                }
            default:
                {
                    break;
                }
        }

        for (int i = 0; i < pressedButtons.Length; i++)
        {
            if (!pressedButtons[i])
            {
                return;
            }
        }
        //Si llega hasta aqui, hemos desbloqueado la ruta alternativa.
        tobeInteractedList[3].SetActive(false);
        tobeInteractedList[4].SetActive(false);
    }

    */


  /*  /// <summary>
    ///  Devuelve el nodo al que pertenece una posicion
    /// </summary>
    /// <param name="pos">Posicion para la que se calcula el nodo</param>
    /// <param name="grafo">Grafo del que se extraera el nodo
    /// <returns></returns>
    public Node GetNodoClick(Vector3 pos, Node[,] grafo)
    {
        int posX = (int)Mathf.Round(pos.x / CG.incrementoX);
        int posZ = (int)Mathf.Round(pos.z / CG.incrementoZ);

        return grafo[posX, posZ];

    }*/

	public void SetUnidadToNode( Unidad objeto)
	{
		GetNodeFromPosition (objeto.transform.position).unidad = objeto;
	}

	public void SetResourceTypeToNode (TipoRecurso type, Vector3 resourcePosition)
	{
		GetNodeFromPosition (resourcePosition).resourceType = type;
	}

    public void LimpiarGrafo(Node[,] nodeMap)
    {
        foreach (Node n in nodeMap)
        {
            if (n != null)
                n.Cost = float.PositiveInfinity;
        }

    }
}
