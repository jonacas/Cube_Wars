using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoverUnidad :  Accion{

    GameObject obj;
    List<Vector3> ruta;
    int posicionActualRuta = 0;
	private const float MOVE_SPEED = 10f;

    float margen = 1.0f; //Margen para indicar que se está lo suficientemente cerca de un punto.

    public bool Ejecutar(GameObject ob, List<Vector3> ruta)
    {
        Debug.LogError("ERROR EN ACCION MOVER: Falta que las unidades sobre los nodos se actualicen");
        Unidad unidadActual = GetComponent<Unidad>();
        /*if (Partida.GetPartidaActual().Jugadores[unidadActual.IdJugador].RestarPuntosDeAccion(costeAccion))
        {*/
            obj = ob;
            this.ruta = ruta; //IMPORTANTE CONTROLAR DESDE FUERA QUE LLEGUE UNA RUTA VIABLE, ES DECIR, QUE EL OBJETIVO ESTÉ AL ALCANCE DE LA UNIDAD QUE SE QUIERE MOVER. AQUI NO SE CONTROLA ESE ERROR.
			
            StartCoroutine("RecorrerRuta");

            return true;
        /*}
        else
            return false;*/
    }

    IEnumerator RecorrerRuta()
    {
        while(posicionActualRuta < ruta.Count-1)
        {
			
			obj.transform.position = Vector3.MoveTowards(obj.transform.position, ruta[posicionActualRuta + 1], Time.deltaTime * MOVE_SPEED);
			if (Vector3.Distance(obj.transform.position, ruta[posicionActualRuta + 1]) < margen)
				posicionActualRuta++;
			else
				yield return null;
        
        }
        posicionActualRuta = 0;
    }

    public override void CancelarAccion()
    {
        throw new System.NotImplementedException();
    }
}
