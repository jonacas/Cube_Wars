using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grafo : MonoBehaviour {

    Dictionary<int,Node> grafo;
    // Grafo es un diccionario de parejas en el que la clave es un numero (el nombre del gameobject) y el Nodo

    void Awake()
    {        
       /* Node[] nodos = this.GetComponentsInChildren<Node>();
        grafo = new Dictionary<int, Node>();

        foreach (Node value in nodos)
        {
            grafo.Add(System.Convert.ToInt32(value.gameObject.name), value);             
        }*/
        //TestingAStar(AEstrella.FindPath(_nodos[7], _nodos[7], 8, false));
    }

    public Node FindNode(int position)
    {
        return grafo[position];
    }

    /*public void TestingAStar(Node[] nodos)
    {
        foreach (Node value in nodos)
            Debug.Log(value.gameObject.name);
    }*/
}
