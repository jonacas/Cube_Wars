using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control : MonoBehaviour {

    /// <summary>
    /// Devuelve una lista de nodos que estan dentro del alcance partiendo de otro nodo
    /// </summary>
    /// <param name="nodoIncicial">El nodo desde el que se parte</param>
    /// <param name="alcance">El alcance maximo</param>
    /// <returns></returns>
    public static List<Node> GetNodosAlAlcance(Node nodoIncicial, int alcance)
    {
        print("GetNodosAlcance");
        List<Node> cerrados = new List<Node>();
        List<Node> pendientes = new List<Node>();
        Node nodoActual;

        pendientes.Add(nodoIncicial);
        pendientes[0].Cost = 0;

        while (pendientes.Count > 0)
        {
            //print("iteracion");
            nodoActual = pendientes[0];
            pendientes.Remove(nodoActual);

            foreach (Pareja vecino in nodoActual.ArrayVecinos)
            {
                //si el nodo no ha sido visitado (coste = infinito) lo cambiamos, si no no
                if (vecino.nodo.Cost > nodoActual.Cost + 1)
                    vecino.nodo.Cost = nodoActual.Cost + 1;
                else
                    continue;
                if (vecino.nodo.Cost < alcance)
                {
                    //evitamos anadir duplicados
                    if(!pendientes.Contains(vecino.nodo))
                        pendientes.Add(vecino.nodo);
                }

            }
            //evitamos anadir duplicados
            if (!cerrados.Contains(nodoActual))
                cerrados.Add(nodoActual);
        }
        cerrados.Remove(nodoIncicial);
        //CheckNodeList(cerrados);
        return cerrados;

    }

    public static void CheckNodeList(List<Node> nodes)
    {
        print("Resultado: " + nodes.Count);
        foreach (Node n in nodes)
        {
            print(n.fil + "/" + n.col);
        }
    }
}
