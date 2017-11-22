using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unidad {

    Vector3 posicion;
    public Vector3 Posicion
    {
        get { return posicion; }
        set { posicion = value; }
    }

    int vida;
    public int Vida
    {
        get { return vida; }
        set { vida = value; }
    }


    float defensa;
    public float Defensa
    {
        get { return defensa; }
        set { defensa = value; }
    }


    /*FALTAN ATRIBUTOS INFLUENCIAS*/

    public Unidad(Vector3 pos, int saludMax, float defensa)
    {
        posicion = pos;
        vida = saludMax;
        this.defensa = defensa;
    }

}
