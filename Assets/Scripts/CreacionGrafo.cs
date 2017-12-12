using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CreacionGrafo : MonoBehaviour {
    
    private const int NUMERO_GRAFOS = 10;


	public int filas, columnas; //definen la cantidad de _nodos del grafo
	public float radioTest;
	/*
	 * 		COLUMNAS (X)
	 * F
	 * I
	 * L
	 * A
	 * S
	 * (Z)
	 * 
	 * */

	public GameObject GO_NodoBase;
	private Vector3 esquina; //esquina complementaria que define el area del grafo
    public Node[,] nodeMap;
	public GameObject GO_Esquina;
	public float incrementoX, incrementoZ;
    public Node[,] grafo1, grafo2;

	//=== VALORES A TENER EN CUENTA PARA UNA CORRECTA CREACION DEL GRAFO:
	//=== ESCALA DE LA CASILLA, SEGUN MODELADO ORIGINAL: 1.075, 1.075, 0.5
	//=== DATOS CREACION DEL GRAFO: 100 filas, 100 columnas, radio test 2
	//== Posicion de esquina: 300, 0, 300

	public GameObject casilla;
	public GameObject[,] tablero;
	/*
	 * public float scaleCasillaX = 1f;
	public float scaleCasillaY = 1f;
	public float scaleCasillaZ = 0.5f;
	*/

    void Awake()
    {
        nodeMap = CrearGrafo();
    }

	/*void Update()
	{
		for (int i = 0; i < filas - 1; i++) 
		{
			for (int j = 0; i < columnas - 1; j++) 
			{
				tablero [i, j].transform.localScale = new Vector3 (scaleCasillaX, scaleCasillaY, scaleCasillaZ);
			}
		}
	}*/

	// Use this for initialization
    public Node[,] CrearGrafo()
    {

        Vector3 testPos = new Vector3();
        testPos.y = this.transform.position.y;
        bool water;
        int obstacleLayer = 1 << 8;
        //mascara de cristal
        obstacleLayer = obstacleLayer | 1 << 9;
        int waterLayer = 1 << 4;

        Node[] vectorAux = new Node[8];
        Node aux;
        Node nodoActual;
        //Node[,] nodeMap;

		GameObject[,] testGameObjectMap;

        //agua.SetActive (false);
        nodeMap = new Node[filas, columnas]; //almacena los _nodos para asignar vecinos

		testGameObjectMap = new GameObject[filas, columnas];
		tablero = new GameObject[filas, columnas];

        esquina = GO_Esquina.transform.position;

        incrementoX = (esquina.x - this.transform.position.x) / columnas;
        incrementoZ = (esquina.z - this.transform.position.z) / filas;

        for (int i = 0; i < filas; i++)
        {
            for (int j = 0; j < columnas; j++)
            {
                testPos.x = this.transform.position.x + incrementoX * j;
                testPos.z = this.transform.position.z + incrementoZ * i;

				//==============TEST DE SPAWNEO DE GRAFO, COMENTAR PARA CODIGO FINAL==============
				/*GameObject testInstance = Instantiate(GO_NodoBase, new Vector3(this.transform.position.x + incrementoX * j, 
					this.transform.position.y, this.transform.position.z + incrementoZ * i),
					GO_NodoBase.transform.rotation); */
				//================================================================================
				//==============SPAWNEO DE EL GRAN TABLERO =======================================
				/*GameObject casillaNueva = Instantiate(casilla, new Vector3(this.transform.position.x + incrementoX * j, 
					this.transform.position.y - 2, this.transform.position.z + incrementoZ * i),
					casilla.transform.rotation);
                casillaNueva.name = "CasillaX" + i + "X" + j; 
                */
				//================================================================================

                //comprobamos si el nodo estaria dentro o tocando un obstaculo
                if (Physics.OverlapSphere(testPos, radioTest, obstacleLayer).Length > 0)
                {
                    nodeMap[i, j] = null;
                    continue;//si colisiona, es descartado
                }

                //comprobacion de si el nodo está en el agua
                //agua.SetActive (true);
                if (Physics.OverlapSphere(testPos, radioTest, waterLayer).Length > 0)
                    water = true;
                else
                    water = false;

                //si no, se instancia
                aux = new Node(new Vector3(testPos.x, testPos.y, testPos.z), water);
                aux.fil = i;
                aux.col = j;
                nodeMap[i, j] = aux;

				/*for (int K = 0; K < StageData.currentInstance.numberOfPlayers; K++)
				{
					nodeMap [i, j].AddPlayerToInfluences ();
				}*/
				//testGameObjectMap [i, j] = testInstance;
				//tablero[i,j] = casillaNueva;
            }
        }

        //recorremos el mapa de _nodos y asignamos los vecinos
        for (int i = 0; i < filas; i++)
        {
            for (int j = 0; j < columnas; j++)
            {
                if (nodeMap[i, j] == null)
                    continue;

                Array.Clear(vectorAux, 0, vectorAux.Length);
                nodoActual = nodeMap[i, j];


                if (i > 0)
                { //fila sup

                    if (comprobarAccesible(nodoActual, nodeMap[i - 1, j]))
                        vectorAux[0] = nodeMap[i - 1, j];
                    if (j > 1)
                        if (comprobarAccesible(nodoActual, nodeMap[i - 1, j - 1]))
                            vectorAux[1] = nodeMap[i - 1, j - 1];
                    if (j < columnas - 1)
                        if (comprobarAccesible(nodoActual, nodeMap[i - 1, j + 1]))
                            vectorAux[2] = nodeMap[i - 1, j + 1];
                }

                //fila inf
                if (i < filas - 1)
                {
                    if (comprobarAccesible(nodoActual, nodeMap[i + 1, j]))
                        vectorAux[3] = nodeMap[i + 1, j];
                    if (j > 1)
                        if (comprobarAccesible(nodoActual, nodeMap[i + 1, j - 1]))
                            vectorAux[4] = nodeMap[i + 1, j - 1];
                    if (j < columnas - 1)
                        if (comprobarAccesible(nodoActual, nodeMap[i + 1, j + 1]))
                            vectorAux[5] = nodeMap[i + 1, j + 1];
                }

                if (j > 0)
                    if (comprobarAccesible(nodoActual, nodeMap[i, j - 1]))
                        vectorAux[6] = nodeMap[i, j - 1];
                if (j < columnas - 1)
                    if (comprobarAccesible(nodoActual, nodeMap[i, j + 1]))
                        vectorAux[7] = nodeMap[i, j + 1];

                nodoActual.SetVecinos(vectorAux);
            }
        }

        /*for (int i = 0; i < filas; i++) {
            for (int j = 0; j < columnas; j++) {
                if(nodeMap[i,j] == null)
                    continue;

                nodoActual = nodeMap [i, j].GetComponent<Node>();

                for(int k = 0; i < 8; i++)
                    vectorAux[k] = null;

                if (i > 0) { //fila sup
                    vectorAux [0] = nodeMap [i - 1, j];
                    if(j > 1)
                        vectorAux [1] = nodeMap [i - 1, j-1];
                    if(j < columnas - 1)
                        vectorAux [2] = nodeMap [i - 1, j+1];
                }

                //fila inf
                if (i < filas - 1) {
                    vectorAux [3] = nodeMap [i + 1, j];
                    if (j > 1)
                        vectorAux [4] = nodeMap [i + 1, j - 1];
                    if (j < columnas - 1)
                        vectorAux [5] = nodeMap [i + 1, j + 1];
                }

                if(j > 0)
                    vectorAux[6] = nodeMap[i, j - 1];
                if(j < columnas - 1)
                    vectorAux[7] = nodeMap[i, j + 1];

                Debug.Log(vectorAux[1].name);
                nodoActual.SetVecinos(vectorAux);
            }
        }*/

        return nodeMap;
    }

	bool comprobarAccesible(Node a, Node b)
	{
		if (b == null)
			return false;
		return !Physics.Raycast (a.position, b.position -
		a.position, Vector3.Distance (a.position, b.position));

	}
}
