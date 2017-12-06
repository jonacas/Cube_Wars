﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Capital : Unidad {

    private const int SALUD_MAX = 500;
    private const int DEFENSA_MAX = 100;
    private const int COMIDA_POR_TURNO = 20;

    int nivel;
    bool posicionAsignada;

    private void Awake()
    {
        saludMaxima = SALUD_MAX;
        Vida = SALUD_MAX;
        acciones = new List<Accion>();
        acciones.Add(GetComponent<CrearUnidad>());
    }

    void Update()
    {
        Nodo = StageData.currentInstance.GetNodeFromPosition(transform.position);
        Nodo.unidad = this;
    }

    public override bool RecibirAtaque(int danoBruto)
    {
 	    bool destruido = base.RecibirAtaque(danoBruto);

        //codigo para informar de destruccion de capital
        if (destruido)
            Partida.GetPartidaActual().DescativarJugadorYComprobarVictoria(IdJugador);
        return destruido;
    }

    public void llenarListaAcciones()
    {
        throw new System.NotImplementedException();
    }
}
