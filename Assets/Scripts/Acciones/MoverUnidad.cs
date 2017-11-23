using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoverUnidad :  Accion{

    GameObject obj;
    Vector3 origen;
    Vector3[] ruta;

    public bool Ejecutar(GameObject ob, Vector3[] ruta)
    {
        obj = ob;
        this.ruta = ruta;
        origen = obj.transform.position;

        return false;
    }
}
