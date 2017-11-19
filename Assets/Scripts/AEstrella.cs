using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Priority_Queue;

public class AEstrella : MonoBehaviour{

    public const bool MAS_PRECISO = true, MAS_RAPIDO = false;

	public List<Vector3> FindPath(Node origin, Node destiny, int capacity, bool precission, bool manhattan, EnemyMovement solicitante, Node[,] copiaNodo)
    {
       // Debug.Log("INICIO de nodo " + origin.gameObject.name + " a ndodo " + destiny.gameObject.name);
		if (origin == destiny) {
			List<Vector3> aux = new List<Vector3> ();
			aux.Add(origin.position);
            solicitante.ResultadoAEstrella(aux);
			return aux;
		}
        StartCoroutine(Work(origin, destiny, capacity, precission, manhattan, solicitante, copiaNodo));
        //Devolver camino
		
        return null; // borrar mas tarde
    }

	private static float heuristicaManhattan(Vector3 origen, Vector3 destino)
	{
		//la suma de las diferencias componente a componente en un espacio en 2 dimensiones
		float diferencia = 0;
		diferencia += Mathf.Abs (destino.x - origen.x);
		diferencia += Mathf.Abs (destino.z - origen.z);
		return diferencia;
	}

    IEnumerator Work(Node origin, Node destiny, int capacity, bool precission, bool manhattan, EnemyMovement solicitante, Node[,] copiaNodo)
    {

        PriorityQueue abiertos = new PriorityQueue(capacity);
        List<Node> cerrados = new List<Node>();
        bool final = false;
        Node actualNode, oldNode, nodoOrigen, nodoDestino;

        nodoOrigen = origin;
        nodoDestino = destiny;
        nodoOrigen.Cost = 0;
        nodoOrigen.Route = null;
        abiertos.Encolar(nodoOrigen, nodoOrigen.Cost);
        actualNode = null;

        int contador = 0;
        int fragmentador = 0;
        while (!final)
        {
            //para evitar bucles (nunca se sabe...)
            if (contador > 10000)
            {
                yield break;
            }

            contador++;
            fragmentador++;
            oldNode = actualNode;
            actualNode = abiertos.Desencolar();
            if (actualNode == null) //si el monticulo se vaica
            {
                actualNode = oldNode;
                break;
            }

            foreach (Pareja value in actualNode.ArrayVecinos)
            {
                //si se llega al nodo con un coste mejor
                if (actualNode.Cost + value.distancia < value.nodo.Cost)
                {
                    //se actualiza el coste
                    value.nodo.Cost = actualNode.Cost + value.distancia;

                    //calculamos nueva prioridad del nodo
                    if (manhattan)
                        value.nodo.Estimated = value.nodo.Cost + heuristicaManhattan(value.nodo.position, destiny.position);
                    else//euclideana
                        value.nodo.Estimated = value.nodo.Cost + Vector3.Distance(value.nodo.position, destiny.position);

                    //actualizar ruta hasta el nodo
                    value.nodo.Route = actualNode;


                    if (value.nodo.QueuePosition == Node.EN_LISTA_CERRADOS) //Si esta en la lista de cerrados, lo sacamos de la lista
                        cerrados.Remove(value.nodo);

                    //comprobamos si ya esta en abiertos
                    if (abiertos.Contiene(value.nodo))
                    {
                        //si esta en la lista, reducimos su prioridad
                        abiertos.ActualizarPrioridad(value.nodo, value.nodo.Estimated);
                    }
                    else
                    {
                        //si no esta, se encola
                        abiertos.Encolar(value.nodo, value.nodo.Estimated);
                    }
                }
            }

            //el nodo que hemos recorrido entra en cerrado
            cerrados.Add(actualNode);
            //Debug.Log("Metido " + actualNode.gameObject.name + " en cerrados");
            actualNode.QueuePosition = Node.EN_LISTA_CERRADOS;
            //Debug.Log("Peek = " + abiertos.Peek().gameObject.name); 


            if (abiertos.NumElementos() == 0 || (precission == false && abiertos.Primero == nodoDestino))
            {
                final = true;
            }
            else if (abiertos.NumElementos() == 0 || abiertos.Primero.Cost > nodoDestino.Cost)
            {
                final = true;
            }

            if (fragmentador >= 5 /*100*/)
            {
                //print(solicitante.gameObject.name + " fragmentando A*");
                fragmentador = 0;
                yield return null;
            }
        }

        abiertos = null;
        cerrados.Clear();

        List<Vector3> path = new List<Vector3>();
        actualNode = destiny;                   //Reciclamos actualNode para usarlo como auxiliar
        while (actualNode != null)
        {
            path.Add(actualNode.position);
            actualNode = actualNode.Route;
        }

        solicitante.ResultadoAEstrella(path);
    }
}
