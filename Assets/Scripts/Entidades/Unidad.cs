using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unidad : MonoBehaviour{

    Vector3 posicion;
    public Vector3 Posicion
    {
        get { return posicion; }
         set { posicion = value; }
    }

    int saludMaxima;
    public int SaludMaxima
    {
        get { return saludMaxima; }
    }


    int vida;
    public int Vida
    {
        get { return vida; }
        set { vida = value; }
    }


    float defensaMaxima;
    public float DefensaMaxima
    {
        get { return defensaMaxima; }
    }

    float defensa;
    public float Defensa
    {
        get { return defensa; }
        set { defensa = value; }
    }

    /*FALTAN ATRIBUTOS INFLUENCIAS*/
    private int idJugador = -1;

    public int IdJugador
    {
        get { return idJugador; }
        set
        {
            if (idJugador == -1)
                idJugador = value;
            else
                Debug.LogError("Se ha intentado modificar un id ya asignado: " + idJugador + " por " + value);
        }
    }


    /*Atributos de habilidad pasiva*/
    /*Esta accion se ejecuta en cada turno sin coste*/
    int alcance;
    IAccion accionPasiva;

    /*public Unidad(Vector3 pos, int saludMax, float defensaMax, int idJugador)
    {
        posicion = pos;
        vida = saludMax;
        saludMaxima = saludMax;
        this.defensa = defensaMax;
        defensaMaxima = defensaMax;
        this.idJugador = idJugador;
    }*/

}
