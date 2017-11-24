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
        Unidad unidadActual = GetComponent<Unidad>();
        if (Partida.GetPartidaActual().Jugadores[unidadActual.IdJugador].RestarPuntosDeAccion(costeAccion))
        {
            obj = ob;
            this.ruta = ruta; //IMPORTANTE CONTROLAR DESDE FUERA QUE LLEGUE UNA RUTA VIABLE, ES DECIR, QUE EL OBJETIVO ESTÉ AL ALCANCE DE LA UNIDAD QUE SE QUIERE MOVER. AQUI NO SE CONTROLA ESE ERROR.

            StartCoroutine("RecorrerRuta");

            return true;
        }
        else
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
