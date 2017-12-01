﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unidad : MonoBehaviour {

    Node nodo; //el nodo sobre el que se situa la unidad;

    Vector3 posicion;
    public Vector3 Posicion
    {
        get { return posicion; }
        set { posicion = value; }
    }

    protected int saludMaxima;
    public int SaludMaxima
    {
        get { return saludMaxima; }
    }

    int danyo;
    public int Danyo
    {
        get { return danyo; }
    }

    public int DanyoContraataque
    {
        get { return danyo / 5; }
    }

    int vida;
    public int Vida
    {
        get { return vida; }
        set { vida = value; }
    }


    protected float defensaMaxima;

    protected List<Vector3> caminoActual;
    protected bool caminoListo;

    internal void ResultadoAEstrella(List<Vector3> aux)
    {
		print ("Camino listo");
        caminoActual = aux;
        caminoListo = true;
    }

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

    //identifica el tipo de unidad para el casting
    protected StageData.UnityType idUnidad;
    public StageData.UnityType IdUnidad
    {
		get{return idUnidad; }
    }

    protected int vision;
    public int Vision
    {
		get{return 0; }
    }
    /*Atributos de habilidad pasiva*/
    /*Esta accion se ejecuta en cada turno sin coste*/    
    protected Accion accionPasiva;

    /*Lista de acciones que se pueden realizar en los turnos*/
    protected List<Accion> acciones;
    public List<Accion> Acciones
    {
		get{ return null;}
    }
    //abstract public void llenarListaAccione(); 

    /*public Unidad(Vector3 pos, int saludMax, float defensaMax, int idJugador)
    {
        posicion = pos;
        vida = saludMax;
        saludMaxima = saludMax;
        this.defensa = defensaMax;
        defensaMaxima = defensaMax;
        this.idJugador = idJugador;
    }*/

    public virtual bool RecibirAtaque(int danoBruto)
    {
        int dano = danoBruto - (int) (danoBruto * defensa / 100);
        if (vida <= 0)
        {
            vida = 0;
            return true;
        }
        else
            return false;
    }


    public void ResaltarCasillasAlAlcance(int alcance)
    {
        //este codigo debe resaltar las casillas del tablero que entran dentro del rango de una de las acciones
    }

    /// <summary>
    /// Solicita un camino y lo recorre cuando esta listo desde su posicion hasta la que se le proporciona
    /// </summary>
    /// <param name="final"></param>
    public virtual void SolicitarYRecorrerCamino(Vector3 final)
    {
        caminoListo = false;
        StageData.currentInstance.GetPathToTarget(this.transform.position, final, this);
    }
}