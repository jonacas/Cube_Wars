using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoverUnidad :  Accion{

    GameObject obj;
    Vector3[] ruta;
    int posicionActualRuta;
    float margen = 1.0f; //Margen para indicar que se está lo suficientemente cerca de un punto.

    public bool Ejecutar(GameObject ob, Vector3[] ruta)
    {

        obj = ob;
        this.ruta = ruta;

        StartCoroutine("RecorrerRuta");

        // a ver como manejamos lo que devolvemos

        return false;

    }

    IEnumerator RecorrerRuta()
    {
        while(posicionActualRuta >= ruta.Length)
        {
            obj.transform.position = Vector3.Lerp(ruta[posicionActualRuta], ruta[posicionActualRuta + 1], Time.deltaTime);
            if (!(Vector3.Distance(obj.transform.position, ruta[posicionActualRuta + 1]) > margen))
            {
                yield return null;
            }
            else
            {
                posicionActualRuta++;
            }
            if (posicionActualRuta > ruta.Length)
            {
                StopCoroutine("RecorrerRuta");
            }
        }
    }
}
