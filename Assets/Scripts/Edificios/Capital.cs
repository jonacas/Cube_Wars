﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Capital :Unidad {

    private const int SALUD_MAX = 500;
    private const int DEFENSA_MAX = 100;
    private const int COMIDA_POR_TURNO = 20;

    int nivel;
    bool posicionAsignada;

    void Awake()
    {
        SetPosicion(transform.position);
    }

    public void SetPosicion(Vector3 pos)    
    {
        if (!posicionAsignada)
        {
            base.Posicion = pos;
            posicionAsignada = true;
        }
        else
            Debug.LogError("Se ha intentado modificar la posicion de la capital: " + IdJugador);
    }
}