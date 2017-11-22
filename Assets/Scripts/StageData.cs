using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageData : MonoBehaviour
{

    public GameObject player;
    public static StageData currentInstance;
    public List<EnemyMovement> enemiesInStage;
    public CreacionGrafo CG;

    private int obstacleLayerMask = 1 << 8;
    private bool cmunicationsEnabeled;
    public bool ComunicationsEnabeled
    {
        get;
        set;
    }

    private AEstrella aStar;

    public GameObject[] tobeInteractedList;
    private bool[] pressedButtons = { false, false, false };

	public enum resourceType
	{
		Army, 
		Resource, 
		Building
	};

    void Awake()
    {
        currentInstance = this;
        ComunicationsEnabeled = true;
        aStar = this.GetComponent<AEstrella>();
    }
    public GameObject GetPlayer()
    {
        return player;
    }
    public List<Vector3> GetPathToTarget(Vector3 requester, Vector3 target, EnemyMovement solicitante)
    {
        List<Transform> camino;
        List<Pareja> listaVecinosAux = new List<Pareja>();

        Node inicio, final;
        int initX = (int)Mathf.Round(requester.x / CG.incrementoX);
        int initZ = (int)Mathf.Round(requester.z / CG.incrementoZ);
        int finalX = (int)Mathf.Round(target.x / CG.incrementoX);
        int finalZ = (int)Mathf.Round(target.z / CG.incrementoZ);

        if (initX < 0 || initX >= CG.filas || initZ < 0 || initZ >= CG.columnas || finalX < 0 || finalX >= CG.filas || finalZ < 0 || finalZ >= CG.columnas)
            return null;

        Node closestNode = null;

        if (solicitante.grafo[initZ, initX] != null)
            inicio = solicitante.grafo[initZ, initX];
        else
            inicio = null;

        if (inicio == null)
        {
            closestNode = null;
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if (solicitante.grafo[initZ + i, initX + j] != null)
                        inicio = solicitante.grafo[initZ + i, initX + j];
                    else
                        inicio = null;

                    if (inicio != null && !Physics.Raycast(solicitante.grafo[initZ + i, initX + j].position, target,
                        Vector3.Distance(solicitante.grafo[initZ + i, initX + j].position, target), obstacleLayerMask))
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

        if (solicitante.grafo[finalZ, finalX] != null)
            final = solicitante.grafo[finalZ, finalX];
        else
            final = null;

        if (final == null)
        {
            closestNode = null;
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if (solicitante.grafo[finalZ + i, finalX + j] != null)
                        final = solicitante.grafo[finalZ + i, finalX + j];
                    else
                        final = null;

                    if (final != null && !Physics.Raycast(solicitante.grafo[finalZ + i, finalX + j].position, target,
                            Vector3.Distance(solicitante.grafo[finalZ + i, finalX + j].position, target), obstacleLayerMask))
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

        aStar.FindPath(final, inicio, CG.filas * CG.columnas, false, true, solicitante, solicitante.grafo);

        return new List<Vector3>();
    }

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


    public void LimpiarGrafo(Node[,] nodeMap)
    {
        foreach (Node n in nodeMap)
        {
            if (n != null)
                n.Cost = float.PositiveInfinity;
        }

    }

}
